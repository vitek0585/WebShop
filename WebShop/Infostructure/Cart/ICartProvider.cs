using System.Web;
using WebShop.Infostructure.Common;
using WebShop.Models;

namespace WebShop.Infostructure.Cart
{
    public interface ICartProvider<TItem>
    {
        ICart<TItem> GetCart();
    }

    class CartProvider:ICartProvider<UserOrder>
    {
        public ICart<UserOrder> GetCart()
        {
            var cart = HttpContext.Current.Session[ValuesApp.Cart];
            if (cart == null)
            {
                cart = new UserCart();
                HttpContext.Current.Session[ValuesApp.Cart] = cart;
            }
            return (UserCart)cart;
        }
    }
}