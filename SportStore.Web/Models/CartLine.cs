namespace SportStore.Web.Models
{
    public class CartLine
    {
        public int CartLineId { get; set; }

        public ProductItem Product { get; set; }

        public int Quantity { get; set; }
    }
}