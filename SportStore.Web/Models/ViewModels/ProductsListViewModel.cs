namespace SportStore.Web.Models.ViewModels
{
    using System.Collections.Generic;

    public class ProductsListViewModel
    {
        public IEnumerable<ProductItem> Products { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string CurrentCategory { get; set; }
    }
}
