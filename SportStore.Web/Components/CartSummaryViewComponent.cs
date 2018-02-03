namespace SportStore.Web.Components
{
    using Microsoft.AspNetCore.Mvc;

    using SportStore.Web.Models;

    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }
        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
