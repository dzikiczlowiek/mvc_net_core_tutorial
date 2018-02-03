namespace SportStore.DataAccess.Products.Indexes
{
    using System;
    using System.Linq.Expressions;

    using SportStore.Common;
    using SportStore.DataAccess.Infrastructure;

    using StackExchange.Redis;

    public class CategoryIndex : RedisValueOrderedIndex<Product, string>
    {
        private const string IndexName = "category";

        public static string Path => $"{Constants.Namespace.Index}:{IndexName}";

        public override Expression<Func<Product, string>> Map { get; } = p => p.Category;

        public override string Name => IndexName;

        public override void AddIndex(IDatabase database, Product entity, string entityKey)
        {
            base.AddIndex(database, entity, entityKey);
            var indexValue = Map.Compile()(entity);
            database.SetAdd($"{Constants.Namespace.Index}:{Name}", indexValue, CommandFlags.FireAndForget);
        }

        protected override double CalculateScore(string value)
        {
            return 1;
        }

        public Func<IDatabase, RedisValue[]> GetKeysForValue(string value, int page, int size = -1)
        {
            Func<IDatabase, RedisValue[]> func = db => db.SortedSetRangeByRank($"{Constants.Namespace.Index}:{IndexName}:{value}",page * size,page * size + size -1);
            return func;
        }

    }
}
