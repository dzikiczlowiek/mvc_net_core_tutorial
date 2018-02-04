namespace SportStore.DataAccess.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;

    using SportStore.Common;
    using SportStore.DataAccess.Infrastructure;
    using SportStore.DataAccess.Products.Indexes;

    using StackExchange.Redis;

    public sealed class ProductsObjectStore : RedisObjectStore<Product>, IProductsObjectStore
    {
        public ProductsObjectStore(IDatabase database)
            : base(database, "products")
        {
        }

        public void Store(Product product)
        {
            var id = product.Id;
            if (!Exist(id))
            { 
                product.Id = GetNextIdentityId();
            }

            this.Save(product.Id, product);
        }

        public IEnumerable<Product> GetAll()
        {
            var index = (ProductUniqueKeyIndex)Indexes[typeof(ProductUniqueKeyIndex)];
            var productIds = index.GetKeys(Database).Select(x => (string)x);
            return GetAll(productIds).ToList();
        }

        public IEnumerable<Product> GetPaged(int page, int pageSize = 10)
        {
            var index = (ProductUniqueKeyIndex)Indexes[typeof(ProductUniqueKeyIndex)];
            var productIds = index.GetRangeOfKeys(page, pageSize)(Database).Select(x => (string)x);
            return GetAll(productIds).ToList();
        }

        public IEnumerable<Product> GetFiltered(string category, int page, int pageSize = 10)
        {
            Func<IDatabase, RedisValue[]> func;
            if (string.IsNullOrEmpty(category))
            {
                var index = (ProductUniqueKeyIndex)Indexes[typeof(ProductUniqueKeyIndex)];
                func = index.GetRangeOfKeys(page, pageSize);
            }
            else
            {
                var index = (CategoryIndex)Indexes[typeof(CategoryIndex)];
                func = index.GetKeysForValue(category, page, pageSize);
            }

            var productIds = func(Database).Select(x => (string)x);
            return GetAll(productIds).ToList();
        }

        public void Remove(long productId)
        {
            if (Exist(productId))
            {
                Delete(productId);      
            }
        }

        public IEnumerable<string> GetAllCategories()
        {
            var results = Database.SetMembers(CategoryIndex.Path).Select(x => (string)x);
            return results;
        }

        protected override void AddIndexes()
        {
            Indexes.Add(typeof(ProductUniqueKeyIndex), new ProductUniqueKeyIndex());
            Indexes.Add(typeof(CategoryIndex), new CategoryIndex());
        }
    }
}
