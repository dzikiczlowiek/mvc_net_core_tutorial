namespace SportStore.Web.Models
{
    using System.Collections.Generic;

    public interface IOrderRepository
    {
        OrderItem Get(long id);

        IEnumerable<OrderItem> GetAll();

        IEnumerable<OrderItem> GetFilteredByShippingStatus(bool shipped, int page, int range);

        void SaveOrder(OrderItem order);
    }
}
