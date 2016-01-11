using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Implements;
using WebShop.Infostructure.Storage.Interfaces;

namespace WebShop.Filters
{
    public class CurrencyAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string currency = null;
            ICookieConsumer storage = new CookieConsumer();

            if (storage.GetValueStorage(filterContext.HttpContext, ValuesApp.Currency) != null)
                currency = storage.GetValueStorage(filterContext.HttpContext, ValuesApp.Currency);
            else
                currency = ValuesApp.CurrencyDefault;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currency);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currency);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

    }
}