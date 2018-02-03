namespace SportStore.DataAccess.Infrastructure
{
    using System;

    using StackExchange.Redis;

    public static class CountingSemaphore
    {
        public static RedisResult Aquire(
            this IDatabase db,
            string semaphoreName,
            int limit,
            int timeout = 10)
        {
            const string ScriptText = "redis.call('zremrangebyscore', @semaphoreName, '-inf', @diff) "
                                      + "if redis.call('zcard', @semaphoreName) < tonumber(@limit) then "
                                      + "redis.call('zadd', @semaphoreName, @now, @identifier) "
                                      + "return ARGV[4] "
                                      + "end";
            var identifier = Guid.NewGuid().ToString();
            var now = DateTime.Now.GetTotalSeconds();
            var diff = now - timeout;
            var script = LuaScript.Prepare(ScriptText);

            var result = db.ScriptEvaluate(script, new { semaphoreName, diff, limit, now, identifier });
            return result;
        }

        public static bool Release(this IDatabase db, string semaphoreName, string identifier)
        {
            return db.SortedSetRemove(semaphoreName, identifier);
        }

        public static RedisResult Refresh(this IDatabase db, string semaphoreName)
        {
            const string ScriptText =
                "if redis.call('zscore', @semaphoreName, @identifier, @now) then "
                + "return redis.call('zadd',@identifier, @now, @identifier) or true " + "end";
            var identifier = Guid.NewGuid().ToString();
            var script = LuaScript.Prepare(ScriptText);
            var result = db.ScriptEvaluate(script, new { semaphoreName, identifier, now = DateTime.Now.GetTotalSeconds() });
            return result;
        }
    }
}
