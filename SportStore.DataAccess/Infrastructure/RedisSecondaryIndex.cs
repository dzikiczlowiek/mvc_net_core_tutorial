namespace SportStore.DataAccess.Infrastructure
{
    using System;
    using System.Linq.Expressions;

    using StackExchange.Redis;

    public abstract class RedisSecondaryIndex<TEntity, TProperty> : IRedisSecondaryIndex<TEntity>
        where TEntity : class
    {
        public abstract Expression<Func<TEntity, TProperty>> Map { get; }

        public abstract string Name { get; }

        public abstract void AddIndex(IDatabase database, TEntity entity, string entityKey);

        public abstract void RemoveIndex(IDatabase database, TEntity entity, string entityKey);

        public abstract string GetIndex();
    }
}
