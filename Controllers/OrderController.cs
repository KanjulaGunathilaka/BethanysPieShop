using BethanysPieShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IShoppingCart _ShoppingCart;

        public OrderController(IOrderRepository orderRepository, IShoppingCart shoppingCart)
        {
            _OrderRepository = orderRepository;
            _ShoppingCart = shoppingCart;

        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
