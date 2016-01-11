using System;
using System.Web.Mvc;
using WebShop.Infostructure.Common;

using WebShop.Infostructure.Storage.Interfaces;

namespace WebShop.Controllers.Base
{
    public class ShopBaseController : System.Web.Mvc.Controller
    {
        protected ICookieConsumer _storage;

        public ShopBaseController(ICookieConsumer storage)
        {
            _storage = storage;
        }

        #region Helper methods
         [NonAction]
        protected string CheckValidReturnUrl(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Uri uri = new Uri(returnUrl);
                if (Url.IsLocalUrl(uri.AbsolutePath))
                    return returnUrl;
            }
            return Url.Action("Index", "Main");
        }
        [NonAction]
        protected string GetCurrentCurrency()
        {
            return _storage.GetValueStorage(HttpContext, ValuesApp.Currency)
                   ?? ValuesApp.CurrencyDefault;
        }
        [NonAction]
        protected string GetCurrentLanguage()
        {
            return _storage.GetValueStorage(HttpContext, ValuesApp.Language) ?? ValuesApp.LanguageDefault;
        }
        #endregion
    }
}