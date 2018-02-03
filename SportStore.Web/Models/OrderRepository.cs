namespace SportStore.Web.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using SportStore.Common;
    using SportStore.DataAccess.Orders;

    public class OrderRepository : IOrderRepository
    {
        private readonly IOrdersObjectStore ordersObjectStore;

        public OrderRepository(IOrdersObjectStore ordersObjectStore)
        {
            this.ordersObjectStore = ordersObjectStore;
        }

        public OrderItem Get(long id)
        {
            return ordersObjectStore.Get(id).Map();
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return ordersObjectStore.GetAll().Select(x => x.Map());
        }

        public IEnumerable<OrderItem> GetFilteredByShippingStatus(bool shipped, int page, int range)
        {
            return ordersObjectStore.GetFilteredByShippingStatus(shipped, page, range).Select(x => x.Map());
        }

        public void SaveOrder(OrderItem order)
        {
            var entity = order.Map();
            ordersObjectStore.Store(entity);
        }
    }
}
