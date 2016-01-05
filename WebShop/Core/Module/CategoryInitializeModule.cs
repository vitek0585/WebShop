using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using WebShop.Repo.Interfaces;

namespace WebShop.Core.Module
{
    public class CategoryInitializeModule : IHttpModule
    {
        
        public void Init(HttpApplication context)
        {
            context.PostUpdateRequestCache += PostHandlerExecute;
        }

        private void PostHandlerExecute(object sender, EventArgs e)
        {
           
            var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
            var http = (HttpApplication) sender;
            //http.Context.Items["Categories"] = categoryService.AllCategory<I>("ru");
            Debug.WriteLine("CategoryInitializeModule");
        }

        public void Dispose()
        {
        }
    }
}