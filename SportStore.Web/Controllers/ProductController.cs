
namespace SportStore.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using SportStore.Web.Models;
    using SportStore.Web.Models.ViewModels;

    public class ProductController : Controller
    {
        private readonly IProductRepository repository;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 0) => View(
            new ProductsListViewModel()
                {
                    Products = repository.Products(category, page, 4),
                    PagingInfo =
                        new PagingInfo()
                            {
                                CurrentPage = page,
                                ItemsPerPage = 4,
                                TotalItems = repository.Products(
                                    category,
                                    0,
                                    int.MaxValue).Count()
                            },
                    CurrentCategory = category
                });
    }
}