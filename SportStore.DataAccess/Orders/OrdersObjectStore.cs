namespace SportStore.DataAccess.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using SportStore.DataAccess.Infrastructure;
    using SportStore.DataAccess.Orders.Indexes;

    using StackExchange.Redis;

    using Order = SportStore.Common.Order;

    public class OrdersObjectStore : RedisObjectStore<Order>, IOrdersObjectStore
    {
        public OrdersObjectStore(IDatabase database)
            : base(database, "orders")
        {
        }

        public void Store(Order order)
        {
            var id = order.Id;
            if (!Exist(id))
            {
                order.Id = GetNextIdentityId();
            }

            this.Save(order.Id, order);
        }

        public IEnumerable<Order> GetAll()
        {
            var index = (OrderUniqueKeyIndex)Indexes[typeof(OrderUniqueKeyIndex)];
            var productIds = index.GetKeys(Database).Select(x => (string)x);
            return GetAll(productIds).ToList();
        }

        public IEnumerable<Order> GetPaged(int page, int pageSize = 10)
        {
            var index = (OrderUniqueKeyIndex)Indexes[typeof(OrderUniqueKeyIndex)];
            var productIds = index.GetRangeOfKeys(page, pageSize)(Database).Select(x => (string)x);
            return GetAll(productIds).ToList();
        }

        public IEnumerable<Order> GetFilteredByShippingStatus(bool shipped, int page, int pageSize = 10)
        {
            var index = (ShippedValueIndex)Indexes[typeof(ShippedValueIndex)];
            var productIds = index.GetRangeOfKeys(shipped, page, pageSize)(Database).Select(x => (string)x);
            return GetAll(productIds).ToList();
        }

        protected override void AddIndexes()
        {
            Indexes.Add(typeof(OrderUniqueKeyIndex), new OrderUniqueKeyIndex());
            Indexes.Add(typeof(ShippedValueIndex), new ShippedValueIndex());
        }
    }
}
