using System.Collections.Generic;
using WebShop.EFModel.Model;

namespace WebShop.Infostructure.Cart
{
    public interface ICart<TItem>
    {
        void AddGood(TItem good);
        IEnumerable<TItem> GetAll();
        bool Update(int id,TItem goods);

        bool Remove(int id);

        void Clear();
    }
}