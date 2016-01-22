using System.Collections.Generic;
using WebShop.EFModel.Model;

namespace WebShop.Infostructure.Cart
{
    public interface ICart<TItem>
    {
        void AddGood(TItem good);
        IEnumerable<TItem> GetAll();
        void SetCountGoods(IEnumerable<TItem> goods);

        bool Remove(TItem id);
  
    }
}