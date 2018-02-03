namespace SportStore.Web.Models
{
    using System.Linq;

    using SportStore.Common;

    public static class Mapper
    {
        public static ProductItem Map(this Product product)
        {
            var item = new ProductItem();
            item.Price = product.Price;
            item.Category = product.Category;
            item.Description = product.Description;
            item.Name = product.Name;
            item.ProductId = product.Id;
            return item;
        }

        public static Product Map(this ProductItem product)
        {
            var item = new Product();
            item.Price = product.Price;
            item.Category = product.Category;
            item.Description = product.Description;
            item.Name = product.Name;
            item.Id = product.ProductId;
            return item;
        }

        public static Order Map(this OrderItem order)
        {
            var item = new Order();
            item.Id = order.OrderId;
            item.Shipped = order.Shipped;
            item.City = order.City;
            item.Country = order.Country;
            item.GiftWrap = order.GiftWrap;
            item.Line1 = order.Line1;
            item.Line2 = order.Line2;
            item.Line3 = order.Line3;
            item.Lines = order.Lines.Select(l => l.Map()).ToList();
            item.Name = order.Name;
            item.State = order.State;
            item.Zip = order.Zip;
            return item;
        }

        public static OrderItem Map(this Order order)
        {
            var item = new OrderItem();
            item.City = order.City;
            item.Country = order.Country;
            item.GiftWrap = order.GiftWrap;
            item.Line1 = order.Line1;
            item.Line2 = order.Line2;
            item.Line3 = order.Line3;
            item.Lines = order.Lines.Select(l => l.Map()).ToList();
            item.Name = order.Name;
            item.OrderId = order.Id;
            item.State = order.State;
            item.Zip = order.Zip;
            return item;
        }

        public static OrderLine Map(this CartLine line)
        {
            var item = new OrderLine();
            item.Product = line.Product.Map();
            item.Quantity = line.Quantity;
            return item;
        }

        public static CartLine Map(this OrderLine line)
        {
            var item = new CartLine();
            item.Product = line.Product.Map();
            item.Quantity = line.Quantity;
            return item;
        }
    }
}
