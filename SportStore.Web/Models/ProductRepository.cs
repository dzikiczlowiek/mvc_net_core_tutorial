namespace SportStore.Web.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using SportStore.DataAccess.Products;

    public class ProductRepository : IProductRepository
    {
        private readonly IProductsObjectStore productsObjectStore;

        public ProductRepository(IProductsObjectStore productsObjectStore)
        {
            this.productsObjectStore = productsObjectStore;
        }

        public IEnumerable<ProductItem> Products() => productsObjectStore.GetAll().Select(Mapper.Map);

        public IEnumerable<ProductItem> Products(int page, int pageSize) =>
            productsObjectStore.GetPaged(page - 1, pageSize).Select(Mapper.Map);

        public IEnumerable<ProductItem> Products(string category, int page, int pageSize = 10) =>
            productsObjectStore.GetFiltered(category, page - 1, pageSize).Select(Mapper.Map);

        public ProductItem GetById(long id) => productsObjectStore.Get(id).Map();

        public IEnumerable<string> Categories() => productsObjectStore.GetAllCategories();

    }
}
