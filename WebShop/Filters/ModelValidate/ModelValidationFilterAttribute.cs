using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebShop.Filters.ModelValidate
{

    public class ModelValidationFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var response = filterContext.RequestContext.HttpContext.Response;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                filterContext.Result = new JsonResult()
                {
                    ContentType = "application/json",
                    Data = filterContext.Controller.ViewData.ModelState.SelectMany(s => s.Value.Errors, (m, e) => e.ErrorMessage),
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}