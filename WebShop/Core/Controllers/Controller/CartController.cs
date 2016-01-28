using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using WebShop.App_GlobalResources;
using WebShop.Core.Controllers.Base;
using WebShop.EFModel.Model;
using WebShop.Filters.Headers;
using WebShop.Filters.ModelValidate;
using WebShop.Infostructure.Cart;
using WebShop.Infostructure.ResponseResult;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Log.Abstract;
using WebShop.Models;
using WebShop.Models.Account;
using WebShop.Repo.Interfaces;
using WebShop.Repo.Model;

namespace WebShop.Core.Controllers.Controller
{
    [RoutePrefix("Cart")]
    public class CartController : ShopBaseController
    {
        private IPurchaseService _purchaseService;
        private ICartProvider<UserOrder> _cartProvider;
        private ILogWriter<string> _log; 
        public CartController(ICookieConsumer storage, ICartProvider<UserOrder> cartProvider, IPurchaseService purchaseService,
            ILogWriter<string> log)
            : base(storage)
        {
            _log = log;
            _cartProvider = cartProvider;
            _purchaseService = purchaseService;
        }
        [Route("")]
        public ActionResult Cart()
        {
            var cart = GetCart();
            var orders = cart.GetAll();
            var data = _purchaseService.GetGoodsByCart<UserOrder>(MapToClassificationGoods(orders), GetCurrentCurrency(), GetCurrentLanguage());
            return View(data);
        }
        [HttpPost]
        [Route("Add")]
        [ModelValidationFilter]
        public JsonResult Add([Bind(Include = "GoodId,SizeId,ColorId,CountGood")]UserOrder order)
        {
            try
            {
                throw new AggregateException(new Exception("Add Test exception"));
                var item = _purchaseService.GetClassification(Mapper.Map<ClassificationGood>(order));
                if (item != null)
                {
                    order.ClassificationId = item.ClassificationId;
                    var cart = GetCart();
                    cart.AddGood(order);
                    return JsonResultCustom(Resource.AddToCartSuccess, HttpStatusCode.Created);
                }
            }
            catch (Exception e)
            {
                _log.LogWriteError("erorr add cart",e);
                return JsonResultCustom(e.Message, HttpStatusCode.BadRequest);

            }

            return JsonResultCustom(Resource.AddToCartError, HttpStatusCode.BadRequest);

        }
        
        [Route("DoOrder")]
        [ModelValidationFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DoOrder(QuickOrderViewModel user)
        {
            PurchaseResult result;
            try
            {
                result = Ordering(null,user.UserName,user.Email,user.PhoneNumber);
                if (result.Success)
                    return JsonResultCustom(Resource.BuySuccess, HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return JsonResultCustom(e.Message, HttpStatusCode.BadRequest);
            }
            if (result.Exception != null)
            {
                _log.LogWriteError("erorr DoOrder cart", result.Exception);
            }
            return JsonResultCustom(result.Error, HttpStatusCode.BadRequest);
        }
        [Route("DoOrderReg")]
        [HttpPost]
        public JsonResult DoOrder()
        {
            PurchaseResult result;
            try
            {
                var id = GetUserId();
                result = Ordering(id);
                if (result.Success)
                    return JsonResultCustom(Resource.BuySuccess, HttpStatusCode.Accepted);
            }
            catch (Exception e)
            {
                return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);
            }
            if (result.Exception != null)
            {
                _log.LogWriteError("erorr DoOrderReg cart", result.Exception);
            }
            return JsonResultCustom(result.Error, HttpStatusCode.BadRequest);
        }
        [Route("Remove")]
        [HttpPost]
        [ModelValidationFilter]
        public JsonResult Remove(int id)
        {
            try
            {
                var cart = GetCart();
                var isDrop = cart.Remove(id);

                if (isDrop)
                    return JsonResultCustom(Resource.Success);
            }
            catch (Exception e)
            {
                _log.LogWriteError("erorr Remove cart", e);
            }
            return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);

        }
        [Route("GetCart")]
        public JsonResult CartGoods()
        {
            var cart = GetCart();
            return JsonResultCustom(cart.GetAll());

        }
        [Route("Details")]
        public JsonResult GetDetailsGoods(int id)
        {
            var data = _purchaseService.GetGoodsDetails<dynamic>(id);

            if (data.Any())
                return JsonResultCustom(data);

            return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);
        }
        [Route("Update")]
        public JsonResult UpdateCart([Bind(Include = "ClassificationId,GoodId,SizeId,ColorId,CountGood")]UserOrder order)
        {
            var item = _purchaseService.GetClassification(Mapper.Map<ClassificationGood>(order));
            if (item != null)
            {
                var cart = GetCart();
                var idToReplace = order.ClassificationId;
                order.ClassificationId = item.ClassificationId;
                var isUpdate = cart.Update(idToReplace, order);
                if (isUpdate)
                    return JsonResultCustom(String.Empty);
            }

            return JsonResultCustom(Resource.AnotherError, HttpStatusCode.BadRequest);
        }
       
        #region Helpers method

        [NonAction]
        private int GetUserId()
        {
            return Request.GetOwinContext().Authentication.User.Identity.GetUserId<int>(); 

        }
        [NonAction]
        private ICart<UserOrder> GetCart()
        {
            return _cartProvider.GetCart();
        }
        [NonAction]
        private static IEnumerable<ClassificationGood> MapToClassificationGoods(IEnumerable<UserOrder> orders)
        {
            return Mapper.Map<IEnumerable<ClassificationGood>>(orders);
        }
        [NonAction]
        private ErrorBuy GutDoOrderErrors()
        {
            return new ErrorBuy()
            {
                NoEnoughtGoods = Resource.NoEnoughtGoods,
                AnotherError = Resource.AnotherError
            };
        }
        [NonAction]
        private PurchaseResult Ordering(int? userId, string userName = null, string phone = null, string email = null)
        {
            var cart = GetCart();
            var orders = cart.GetAll();
            var errors = GutDoOrderErrors();
            var result = _purchaseService.MakeAnOrder(Mapper.Map<IEnumerable<ClassificationGood>>(orders), userId, errors,
                userName, phone, email);
            if (result.Success)
            {
                cart.Clear();
            }
            return result;

        }
        #endregion

    }


}