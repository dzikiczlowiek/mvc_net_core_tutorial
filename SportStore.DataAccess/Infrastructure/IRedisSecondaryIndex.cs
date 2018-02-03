namespace SportStore.DataAccess.Infrastructure
{
    using StackExchange.Redis;

    public interface IRedisSecondaryIndex<in TEntity>
        where TEntity : class
    {
        void AddIndex(IDatabase database, TEntity entity, string entityKey);

        void RemoveIndex(IDatabase database, TEntity entity, string entityKey);
    }
}
