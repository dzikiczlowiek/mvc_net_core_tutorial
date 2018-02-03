namespace SportStore.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using SportStore.Web.Models;

    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;

        private readonly Cart cart;

        public OrderController(IOrderRepository orderRepository, Cart cart)
        {
            this.orderRepository = orderRepository;
            this.cart = cart;
        }

        public ViewResult Checkout() => View(new OrderItem());

        public ViewResult List() => View(orderRepository.GetFilteredByShippingStatus(false, 0, int.MaxValue));

        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {
            OrderItem order = orderRepository.Get(orderID);
            if (order != null)
            {
                order.Shipped = true;
                orderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        public IActionResult Checkout(OrderItem order)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                orderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
