using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebShop.EFModel.Model;
using WebShop.Models;

namespace WebShop.Infostructure.Cart
{
    

    public class UserCart : ICart<UserOrder>
    {
        private ICollection<UserOrder> _goods;

        public UserCart()
        {
            _goods = new List<UserOrder>();

        }

        public void AddGood(UserOrder good, dynamic origin)
        {
            var comparer = new ComparerClassificationGood();
            var target = _goods.FirstOrDefault(g => comparer.Equals(g, good));

            if (target != null)
                target.CountGood += good.CountGood;
            else
            {
                good.ClassificationId = origin.ClassificationId;
                good.ColorName = origin.ColorName;
                good.SizeName = origin.SizeName;
                good.GoodName = origin.GoodName;
                good.Photos = origin.Photos;
                _goods.Add(good);
            }

        }

        public IEnumerable<UserOrder> GetAll()
        {
            return _goods;
        }

        public void SetCountGoods(IEnumerable<UserOrder> goods)
        {
            var comparer = new ComparerClassificationGood();
            foreach (var good in goods)
            {
                var target = _goods.FirstOrDefault(g => comparer.Equals(g, good));
                if (target != null)
                {
                    target.CountGood = good.CountGood;
                }
            }

        }

        public IEnumerable<int> GetAllGoodsId()
        {
            return _goods.Select(g => g.GoodId).Distinct();
        }
        public void RenewPriceGoods(IEnumerable<Good> goods)
        {
            foreach (var good in goods)
            {
                Array.ForEach(_goods.Where(g => g.GoodId == good.GoodId).ToArray(),
                    g => g.PriceUsd = good.PriceUsd);
            }

        }
        public IEnumerable<SalePos> GetSalePoses()
        {
            return Mapper.Map<IEnumerable<SalePos>>(_goods);
        }

        public Sale GetSale(string userName)
        {
            var sale = new Sale()
            {
                DateSale = DateTime.Now,
                Summa = _goods.Sum(n => n.PriceUsd * n.CountGood),
                UserName = userName
            };
            return sale;
        }
        public UserOrder Remove(UserOrder id)
        {
            var comparer = new ComparerClassificationGood();
            var good = _goods.FirstOrDefault(i => comparer.Equals(i, id));
            if (good != null)
            {
                _goods.Remove(good);
                return good;
            }
            throw new ArgumentException(String.Format("No good by id {0}", id.ToString()));
        }
        #region implements IEqualityComparer
        public class ComparerClassificationGood : IEqualityComparer<UserOrder>
        {
            public bool Equals(UserOrder x, UserOrder y)
            {
                return GetHashCode(x) == GetHashCode(y) && x.ColorId == y.ColorId && x.SizeId == y.SizeId;
            }

            public int GetHashCode(UserOrder obj)
            {
                var hash = obj.GoodId.GetHashCode();
                return (hash << 20) * 2 ^ hash;
            }
        }
        #endregion
    }

}