namespace SportStore.DataAccess.Infrastructure
{
    using StackExchange.Redis;

    public abstract class RedisUniqueIndex<TEntity, TProperty> : RedisSecondaryIndex<TEntity, TProperty>
        where TEntity : class
    {
        public override void AddIndex(
            IDatabase database,
            TEntity entity,
            string entityKey)
        {
            var indexKey = $"{Constants.Namespace.Index}:{Name}";
            database.SetAdd(indexKey, entityKey, CommandFlags.FireAndForget);
        }

        public override string GetIndex()
        {
            return $"{Constants.Namespace.Index}:{Name}";
        }
    }
}
