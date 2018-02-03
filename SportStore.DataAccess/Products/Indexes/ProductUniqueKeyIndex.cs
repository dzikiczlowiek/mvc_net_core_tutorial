namespace SportStore.DataAccess.Products.Indexes
{
    using System;
    using System.Linq.Expressions;

    using SportStore.Common;
    using SportStore.DataAccess.Infrastructure;

    using StackExchange.Redis;

    public class ProductUniqueKeyIndex : RedisOrderedIndex<Product, long>
    {
        private const string IndexName = "ProductKey";

        public static string Path => $"{Constants.Namespace.Index}:{IndexName}";

        public override Expression<Func<Product, long>> Map { get; } = x => x.Id;

        public override string Name => IndexName;

        public Func<IDatabase, RedisValue[]> GetKeys { get; } = db => db.SortedSetRangeByRank($"{Constants.Namespace.Index}:{IndexName}");

        public Func<IDatabase, RedisValue[]> GetRangeOfKeys(int page, int range)
        {
            RedisValue[] Func(IDatabase db) => db.SortedSetRangeByRank(Path, page * range, page * range + range - 1);
            return Func;
        }

        protected override double CalculateScore(long value)
        {
            return value;
        }
    }
}
