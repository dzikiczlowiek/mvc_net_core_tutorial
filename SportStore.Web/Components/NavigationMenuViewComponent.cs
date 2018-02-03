namespace SportStore.Web.Components
{
    using Microsoft.AspNetCore.Mvc;

    using SportStore.Web.Models;

    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IProductRepository productRepository;

        public NavigationMenuViewComponent(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(productRepository.Categories());
        }
    }
}
