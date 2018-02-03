namespace TestApp.Console
{
    using System.Linq;

    using Bogus;

    using SportStore.Common;
    using SportStore.DataAccess.Products;

    using StackExchange.Redis;

    class Program
    {
        static void Main(string[] args)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379"))
            {
                var db = redis.GetDatabase(1);
                var objectStore = new ProductsObjectStore(db);
                FeedDataBase(objectStore);
                //var p1 = objectStore.GetPaged(0).ToList();
                //var p2 = objectStore.GetPaged(1).ToList();
            }
        }

        private static void FeedDataBase(ProductsObjectStore objectStore)
        {
            var productFaker = new Faker<Product>();
            productFaker.Rules(
                (f, p) =>
                    {
                        p.Category = f.Commerce.ProductAdjective();
                        p.Description = f.Lorem.Text();
                        p.Name = f.Commerce.ProductName();
                        p.Price = decimal.Parse(f.Commerce.Price());
                    });
            foreach (var product in productFaker.Generate(50))
            {
                objectStore.Store(product);
            }
        }
    }
}
