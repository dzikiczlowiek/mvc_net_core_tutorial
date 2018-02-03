namespace SportStore.DataAccess.Orders.Indexes
{
    using System;
    using System.Linq.Expressions;

    using SportStore.DataAccess.Infrastructure;

    using StackExchange.Redis;

    using Order = SportStore.Common.Order;

    public class OrderUniqueKeyIndex : RedisOrderedIndex<Order, long>
    {
        private const string IndexName = "OrderKey";

        public static string Path => $"{Constants.Namespace.Index}:{IndexName}";

        public override Expression<Func<Order, long>> Map { get; } = o => o.Id;

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
