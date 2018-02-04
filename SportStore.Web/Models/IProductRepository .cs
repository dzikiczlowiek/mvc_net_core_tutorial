namespace SportStore.Web.Models
{
    using System.Collections.Generic;

    public interface IProductRepository
    {
        IEnumerable<ProductItem> Products();

        IEnumerable<ProductItem> Products(int page, int pageSize = 10);

        IEnumerable<ProductItem> Products(string category, int page, int pageSize = 10);

        ProductItem GetById(long id);

        void Store(ProductItem product);

        IEnumerable<string> Categories();

        ProductItem Delete(long productId);
    }
}
