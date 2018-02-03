namespace SportStore.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using SportStore.Web.Models;
    using SportStore.Web.Models.ViewModels;

    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly Cart cart;

        public CartController(IProductRepository productRepository, Cart cart)
        {
            this.productRepository = productRepository;
            this.cart = cart;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
                            {
                                Cart = cart,
                                ReturnUrl = returnUrl
                            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            ProductItem product = productRepository.GetById(productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId,
                                                     string returnUrl)
        {
            ProductItem product = productRepository.GetById(productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}