using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using WebShop.Log.Abstract;

namespace WebShop.Core.Module
{
    public class ErrorHandlerModule : IHttpModule
    {
        Lazy<ILogWriter<string>> _log =
            new Lazy<ILogWriter<string>>(() => DependencyResolver.Current.GetService<ILogWriter<string>>());
        public void Init(HttpApplication context)
        {
            context.Error += ErrorHandler;
        }

        private void ErrorHandler(object sender, EventArgs e)
        {
            var exc = HttpContext.Current.Server.GetLastError();
            if (exc is HttpException && ((HttpException)exc).GetHttpCode() == 404)
            {
                return;
            }

            _log.Value.LogWriteError("error handler", exc);
        }

        public void Dispose()
        {
            ((IDisposable)_log.Value).Dispose();
        }
    }
}