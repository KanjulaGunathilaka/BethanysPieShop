using BethanysPieShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _OrderRepository;
        private readonly IShoppingCart _ShoppingCart;

        public OrderController(IOrderRepository orderRepository, IShoppingCart shoppingCart)
        {
            _OrderRepository = orderRepository;
            _ShoppingCart = shoppingCart;

        }
        public IActionResult Checkout() { return View(); }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _ShoppingCart.GetShoppingCartItems();
            _ShoppingCart.ShoppingCartItems = items;

            if (_ShoppingCart.ShoppingCartItems.Count ==0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some pies first");
            }
            if (ModelState.IsValid) 
            { 
                _OrderRepository.CreateOrder(order);
                _ShoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);
        }
        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order";
            return View();
        }
    }
}
