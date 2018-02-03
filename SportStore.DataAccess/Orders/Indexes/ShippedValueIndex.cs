namespace SportStore.DataAccess.Orders.Indexes
{
    using System;
    using System.Linq.Expressions;

    using SportStore.DataAccess.Infrastructure;

    using StackExchange.Redis;

    using Order = SportStore.Common.Order;

    public class ShippedValueIndex : RedisValueIndex<Order, bool>
    {
        public override Expression<Func<Order, bool>> Map { get; } = order => order.Shipped;

        private const string IndexName = "is_shipped";

        public override string Name => IndexName;

        public static string Path => $"{Constants.Namespace.Index}:{IndexName}";

        public Func<IDatabase, RedisValue[]> GetRangeOfKeys(bool shipped, int page, int range)
        {
            //RedisValue[] Func(IDatabase db) => db.SortedSetRangeByRank($"{Path}:{shipped}", page * range, page * range + range - 1);
            RedisValue[] Func(IDatabase db) => db.SetMembers($"{Path}:{shipped}");
            return Func;
        }
    }
}
