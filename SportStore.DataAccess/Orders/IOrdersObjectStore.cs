namespace SportStore.DataAccess.Orders
{
    using System.Collections.Generic;

    using SportStore.Common;

    public interface IOrdersObjectStore
    {
        Order Get(long id);

        void Store(Order order);

        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetPaged(int page, int pageSize = 10);

        IEnumerable<Order> GetFilteredByShippingStatus(bool shipped, int page, int pageSize = 10);
    }
}
