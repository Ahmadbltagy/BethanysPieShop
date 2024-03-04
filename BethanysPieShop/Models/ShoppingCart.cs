using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly AppDbContext _context;
        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        public ShoppingCart(AppDbContext context)
        {
            this._context = context;
        }

        public static ShoppingCart GetCart(IServiceProvider service){
            ISession? session = service.GetRequiredService<IHttpContextAccessor>()?
                                      .HttpContext?
                                      .Session;
            
            var context = service.GetService<AppDbContext>() ?? throw new Exception("Error initializing");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId",cartId);

            return new ShoppingCart(context){ShoppingCartId = cartId};
        }

        public void AddToCart(Pie pie){
            var shoppingCartItem = _context.ShoppingCartItems
                                           .SingleOrDefault(s=> s.ShoppingCartId == ShoppingCartId && s.Pie.PieId == pie.PieId);
            
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem{
                    ShoppingCartId = ShoppingCartId,
                    Pie = pie,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _context.SaveChanges();
        }

        public int RemoveFromCart(Pie pie)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                                           .SingleOrDefault(s=>s.ShoppingCartId == ShoppingCartId && s.Pie.PieId == pie.PieId);
            
            int localAmount = 0;
            if(shoppingCartItem !=null){
                if(shoppingCartItem.Amount > 1){
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }else{
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            _context.SaveChanges();
            
            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems(){
            return ShoppingCartItems ??= _context.ShoppingCartItems
                                                .Where(p=>p.ShoppingCartId == ShoppingCartId)
                                                .Include(s=>s.Pie)
                                                .ToList();
        }

        public void ClearCart(){
            var cartItems = _context.ShoppingCartItems
                                    .Where(s=>s.ShoppingCartId == ShoppingCartId);
            
            _context.ShoppingCartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

       public decimal GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItems
                                .Where(c => c.ShoppingCartId == ShoppingCartId)
                                .Select(c => c.Pie.Price * c.Amount)
                                .Sum();
            return total;
        }
    }
}