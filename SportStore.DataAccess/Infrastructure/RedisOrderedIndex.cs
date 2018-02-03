namespace SportStore.DataAccess.Infrastructure
{
    using StackExchange.Redis;

    public abstract class RedisOrderedIndex<TEntity, TProperty> : RedisSecondaryIndex<TEntity, TProperty>
        where TEntity : class
    {
        public override void AddIndex(IDatabase database, TEntity entity, string entityKey)
        {
            var indexValue = Map.Compile()(entity);
            database.SortedSetAdd(GetIndex(), entityKey, CalculateScore(indexValue), CommandFlags.FireAndForget);
        }

        public override void RemoveIndex(IDatabase database, TEntity entity, string entityKey)
        {
            database.SortedSetRemove(GetIndex(), entityKey, CommandFlags.FireAndForget);
        }

        public override string GetIndex()
        {
            return $"{Constants.Namespace.Index}:{Name}";
        }

        protected abstract double CalculateScore(TProperty value);
    }
}
