namespace SportStore.DataAccess.Products
{
    using System.Collections.Generic;

    using SportStore.Common;

    public interface IProductsObjectStore
    {
        void Store(Product product);

        Product Get(long id);

        IEnumerable<Product> GetAll();

        IEnumerable<string> GetAllCategories();

        IEnumerable<Product> GetPaged(int page, int pageSize = 10);

        IEnumerable<Product> GetFiltered(string category, int page, int pageSize = 10);
    }
}
