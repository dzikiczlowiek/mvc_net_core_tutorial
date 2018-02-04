namespace SportStore.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using SportStore.Web.Models;

    public class AdminController : Controller
    {
        private readonly IProductRepository productRepository;

        public AdminController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public ViewResult Index() => View(productRepository.Products());

        public ViewResult Create() => View("Edit", new ProductItem());

        public ViewResult Edit(int productId) => View(productRepository.GetById(productId));

        [HttpPost]
        public IActionResult Edit(ProductItem product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Store(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            ProductItem deletedProduct = productRepository.Delete(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}