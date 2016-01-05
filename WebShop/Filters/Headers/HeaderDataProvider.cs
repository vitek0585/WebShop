using System.Web.Mvc;
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
            var storage = DependencyResolver.Current.GetService<ICookieConsumer>();
            var lang = storage.GetValueStorage(filterContext.HttpContext, ValuesProvider.Language) ?? ValuesProvider.LanguageDefault;
            var categoryService = DependencyResolver.Current.GetService<ICategoryService>();
            filterContext.Controller.ViewBag.Categories = categoryService.AllCategory<ITypeCategoryModel<ICategoryModelBase>>(lang);
        }
    }
}