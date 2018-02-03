namespace SportStore.DataAccess.Infrastructure
{
    using System;

    using StackExchange.Redis;

    public abstract class RedisValueIndex<TEntity, TProperty> : RedisSecondaryIndex<TEntity, TProperty>
        where TEntity : class
    {
        public override void AddIndex(
            IDatabase database,
            TEntity entity,
            string entityKey)
        {
            var indexValue = Map.Compile()(entity);
            var indexKey = $"{Constants.Namespace.Index}:{Name}:{indexValue}";
            database.SetAdd(indexKey, entityKey, CommandFlags.FireAndForget);
        }

        public override void RemoveIndex(IDatabase database, TEntity entity, string entityKey)
        {
            var indexValue = Map.Compile()(entity);
            var indexKey = $"{Constants.Namespace.Index}:{Name}:{indexValue}";
            database.SetRemove(indexKey, entityKey, CommandFlags.FireAndForget);
        }


        public override string GetIndex()
        {
            return $"{Constants.Namespace.Index}:{Name}";
        }
    }

    public abstract class RedisValueOrderedIndex<TEntity, TProperty> : RedisOrderedIndex<TEntity, TProperty>
        where TEntity : class
    {
        public override void AddIndex(
            IDatabase database,
            TEntity entity,
            string entityKey)
        {
            var indexValue = Map.Compile()(entity);
            var indexKey = $"{Constants.Namespace.Index}:{Name}:{indexValue}";
            database.SortedSetAdd(indexKey, entityKey, 1, CommandFlags.FireAndForget);
        }

        public override void RemoveIndex(IDatabase database, TEntity entity, string entityKey)
        {
           // TODO: not needed?
        }

        public override string GetIndex()
        {
            return $"{Constants.Namespace.Index}:{Name}";
        }
    }
}
