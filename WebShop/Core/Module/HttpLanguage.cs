using System.Web;
using System.Web.Mvc;
using WebShop.Infostructure.Storage.Interfaces;

namespace WebShop.Core.Module
{
    public class HttpLanguage:IHttpModule
    {

        public void Init(HttpApplication context)
        {
            ICookieConsumer storage = DependencyResolver.Current.GetService<ICookieConsumer>();

            //var language = storage.GetValueStorage(context.Request.HttpContext, ValuesProvider.Language) ??
            //           ValuesProvider.LanguageDefault;

            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

            //var currency = storage.GetValueStorage(filterContext.HttpContext, ValuesProvider.Currency) ??
            //           ValuesProvider.CurrencyDefault;

            //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = ValuesProvider.CurrencySymbol[currency].ToString();
            //Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencySymbol = ValuesProvider.CurrencySymbol[currency].ToString();
        }

        public void Dispose()
        {
        }
    }
}