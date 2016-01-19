using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;

namespace WebShop.Core.Controllers.Base
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
        protected string CheckValidReturnUrlAjax(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Uri uri = new Uri(returnUrl);
                if (Url.IsLocalUrl(uri.AbsolutePath))
                    return returnUrl;
            }
            return Url.Action("Index", "Main", null, "http");
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
            return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
        }
        [NonAction]
        protected IEnumerable<string> ReturnErrorModelState()
        {
            return ModelState.Values.SelectMany(e => e.Errors, (m, e) => e.ErrorMessage).ToArray();
        }

        #endregion

    }
}