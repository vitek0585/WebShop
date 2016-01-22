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

        public void AddGood(UserOrder good)
        {
            var comparer = new ComparerUserOrder();
            var target = _goods.FirstOrDefault(g => comparer.Equals(g, good));

            if (target != null)
                target.CountGood += good.CountGood;
            else
            {
                _goods.Add(good);
            }

        }

        public IEnumerable<UserOrder> GetAll()
        {
            return _goods;
        }

        public void SetCountGoods(IEnumerable<UserOrder> goods)
        {
            var comparer = new ComparerUserOrder();
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


        public bool Remove(UserOrder order)
        {
            var compare = new ComparerUserOrder();
            var good = _goods.FirstOrDefault(i => compare.Equals(i, order));
            if (good != null)
            {
                _goods.Remove(good);
                return true;
            }
            return false;
        }

        #region implements IEqualityComparer
        public class ComparerUserOrder : IEqualityComparer<UserOrder>
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