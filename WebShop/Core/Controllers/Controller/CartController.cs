using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;
using WebShop.Controllers.Base;
using WebShop.EFModel.Model;
using WebShop.Filters.Culture;
using WebShop.Infostructure.Cart;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.ResponseResult;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.Controller
{
    [TypeOfCulture]
    public class CartController : ShopBaseController
    {
        private ISalePosRepository _salePos;
        private IGoodsRepository _goods;
        private ISaleRepository _sale;
        private IUnitOfWork _unit;
        private IClassificationGoodRepository _typeGood;

      
        private ICartProvider<UserOrder> _provider; 
        public CartController(ISalePosRepository salePos, ISaleRepository sale, IGoodsRepository good, IUnitOfWork unit,
            IClassificationGoodRepository typeGood, ICookieConsumer storage, ICartProvider<UserOrder> provider)
            : base(storage)
        {
            _provider = provider;

  
            _typeGood = typeGood;
            _goods = good;
            _unit = unit;
            _salePos = salePos;
            _sale = sale;
        }
        public ActionResult ShowCart()
        {
            var cart = GetCart();
            var orders = cart.GetAllGoodsId();
            cart.RenewPriceGoods(_goods.FindBy(g => orders.Contains(g.GoodId)));
            return View(cart.GetAll());
        }

        [HttpPost]
        public ActionResult Add([Bind(Include = "GoodId,SizeId,ColorId,CountGood")]UserOrder data)
        {
            try
            {
                Expression<Func<ClassificationGood, bool>> predicat = (g) => g.GoodId == data.GoodId && g.SizeId == data.SizeId &&
                    g.ColorId == data.ColorId;

                var originType = _typeGood.GetByExpressionSelect(predicat,
                    (c) => new
                    {
                        c.Size.SizeName,
                        c.ClassificationId,
                        c.Color.ColorName,
                        GoodName = c.Good.GoodNameRu,
                        Photos = c.Good.Image.Select(p => p.ImageId)
                    }, (c) => c.Color, (c) => c.Size, (c) => c.Good, (c) => c.Good.Image);

                if (originType == null)
                    throw new ArgumentException();

                var cart = GetCart();
                cart.AddGood(data, originType);

                return new HttpStatusCodeResult(HttpStatusCode.Created,
                    String.Format("Product id:{0} was added to cart!", data.GoodId));
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                    String.Format("Product id:{0} not was added to cart!", data.GoodId));
            }
        }

        [HttpPost]
        public ActionResult Buy(ICollection<UserOrder> data, string userName)
        {
            var cart = GetCart();
            cart.SetCountGoods(data);
            var orders = cart.GetAllGoodsId();
            cart.RenewPriceGoods(_goods.FindBy(g => orders.Contains(g.GoodId)));
            try
            {
                _unit.StartTransaction();
                var sale = _sale.Add(cart.GetSale(userName));
                _salePos.Add(cart.GetSalePoses(), sale);
                _unit.Save();
                _unit.Commit();
            }
            catch (ArgumentException e)
            {
                _unit.Rollback();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception e)
            {
                _unit.Rollback();
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Your order has not been added");

            }
            return new HttpStatusCodeResult(HttpStatusCode.Accepted, "Your order has been successfully added");
        }
        [HttpPost]
        public JsonResult Remove(UserOrder id)
        {
            UserOrder good = null;
            var cart = GetCart();
            try
            {
                good = cart.Remove(id);
            }
            catch (ArgumentException e)
            {
                SetStatusResponse(HttpStatusCode.BadRequest, e.Message);
                return null;
            }

            SetStatusResponse(HttpStatusCode.Accepted, String.Format("{0} has been removed", good.GoodName));
            return Json(cart.GetAll().Count());
        }
        [HttpPost]
        public JsonResult UserCart()
        {
            var currency = GetCurrentCurrency();
            return new JsonResultCustom(GetCart().GetAll().Select(g => new
            {
                photos = g.Photos,
                goodCount = g.CountGood,
                goodId = g.GoodId,
                goodName = g.GoodName,
                //priceUsd = _converter.ConvertWithCeiling(g.PriceUsd, currency),
                size = new { g.SizeId, g.SizeName },
                color = new { g.ColorId, g.ColorName },
                currencyExtend = currency
            }));

        }
        #region Helpers method
        [NonAction]
        private void SetStatusResponse(HttpStatusCode status, string description)
        {
            Response.StatusCode = (int)status;
            Response.StatusDescription = description;
        }
        [NonAction]
        private string GetCurrentCurrency()
        {
            return _storage.GetValueStorage(ControllerContext.HttpContext, ValuesProvider.Currency)
                   ?? ValuesProvider.CurrencyDefault;
        }
        [NonAction]
        private ICart<UserOrder> GetCart()
        {
            return _provider.GetCart();
        }

        [NonAction]
        protected override void Dispose(bool disposing)
        {
            _unit.Dispose();
            base.Dispose(disposing);
        }

        #endregion

    }


}