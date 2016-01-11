using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;

namespace WebShop.Filters.Culture
{
    public class TypeOfCultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ICookieConsumer storage = DependencyResolver.Current.GetService<ICookieConsumer>();

            var language = storage.GetValueStorage(filterContext.HttpContext, ValuesApp.Language) ??
                       ValuesApp.LanguageDefault;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

            var currency = storage.GetValueStorage(filterContext.HttpContext, ValuesApp.Currency) ??
                       ValuesApp.CurrencyDefault;

            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = ValuesApp.GetCurrencySymbol(language,currency);
            Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencySymbol = ValuesApp.GetCurrencySymbol(language, currency);
   
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
        }
    }
}