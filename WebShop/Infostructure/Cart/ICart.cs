using System.Collections.Generic;
using WebShop.EFModel.Model;

namespace WebShop.Infostructure.Cart
{
    public interface ICart<TItem>
    {
        void AddGood(TItem good, dynamic origin);
        IEnumerable<TItem> GetAll();
        void SetCountGoods(IEnumerable<TItem> goods);
        IEnumerable<int> GetAllGoodsId();
        void RenewPriceGoods(IEnumerable<Good> goods);
        IEnumerable<SalePos> GetSalePoses();
        Sale GetSale(string userName);
        TItem Remove(TItem id);
    }
}