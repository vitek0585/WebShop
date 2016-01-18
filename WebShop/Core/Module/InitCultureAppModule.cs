using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Repo.Interfaces;

namespace WebShop.Core.Module
{
    public class InitCultureAppModule : IHttpModule
    {
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += InitCulture;
        }

        private void InitCulture(object sender, EventArgs e)
        {
            ICookieConsumer storage = DependencyResolver.Current.GetService<ICookieConsumer>();
            var context = HttpContext.Current;
            var language = storage.GetValueStorage(context.Request.Cookies, ValuesApp.Language) ??
                       ValuesApp.LanguageDefault;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(language);

            var currency = storage.GetValueStorage(context.Request.Cookies, ValuesApp.Currency) ??
                       ValuesApp.CurrencyDefault;

            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol = ValuesApp.GetCurrencySymbol(language, currency);
            Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencySymbol = ValuesApp.GetCurrencySymbol(language, currency);
        }

       
        public void Dispose()
        {
        }
    }
}