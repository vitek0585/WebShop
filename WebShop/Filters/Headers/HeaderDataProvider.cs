using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Filters.Headers
{
    public class HeaderDataProvider : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoExecuteFilterHeaderDataProviderAttribute), false).Any())
            {
                return;
            }
            if (!filterContext.IsChildAction)
            {
                var storage = DependencyResolver.Current.GetService<ICookieConsumer>();
                var lang = storage.GetValueStorage(filterContext.HttpContext, ValuesApp.Language) ??
                           ValuesApp.LanguageDefault;
                var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
                filterContext.Controller.ViewBag.Categories =
                    categoryService.AllCategory<ITypeCategoryModel<ICategoryModelBase>>(lang);
            }
        }
    }

  
    public class NoExecuteFilterHeaderDataProviderAttribute : ActionFilterAttribute
    {
        
        
    }
}