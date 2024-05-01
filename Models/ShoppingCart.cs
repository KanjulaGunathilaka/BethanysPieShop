using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace BethanysPieShop.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        private ShoppingCart(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            BethanysPieShopDbContext context = services.GetService<BethanysPieShopDbContext>() ?? throw new Exception("Error Initailizing");
            String cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }
        public void AddToCart(Pie pie)
        {
            var ShoppingCartItem = _bethanysPieShopDbContext.ShoppingCartItems.SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            if (ShoppingCartItem == null)
            {
                ShoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };
                _bethanysPieShopDbContext.ShoppingCartItems.Add(ShoppingCartItem);
            }
            else
            {
                ShoppingCartItem.Amount++;

            }
            _bethanysPieShopDbContext.SaveChanges();
        }
        public int RemoveFromCart(Pie pie)
        {
            var ShoppingCartItem = _bethanysPieShopDbContext.ShoppingCartItems.SingleOrDefault(s => s.Pie.PieId == pie.PieId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;
            if (ShoppingCartItem != null)
            {
                if (ShoppingCartItem.Amount > 1)
                {
                    ShoppingCartItem.Amount--;
                    localAmount = ShoppingCartItem.Amount;
                }

                else
                {
                    _bethanysPieShopDbContext.ShoppingCartItems.Remove(ShoppingCartItem);
                }

            }
            _bethanysPieShopDbContext.SaveChanges();
            return localAmount;
        }
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= _bethanysPieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId).Include(s => s.Pie).ToList();
        }
        public void ClearCart()
        {
            var CartItems = _bethanysPieShopDbContext.ShoppingCartItems.Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _bethanysPieShopDbContext.ShoppingCartItems.RemoveRange(CartItems);
            _bethanysPieShopDbContext.SaveChanges(true);
        }
        public decimal GetShoppingCartTotal()
        {
            var total = _bethanysPieShopDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId).Select(c => c.Pie.Price * c.Amount).Sum();
            return total;
        }


    }

}
