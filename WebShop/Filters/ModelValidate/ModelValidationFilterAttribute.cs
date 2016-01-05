using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebShop.Filters.ModelValidate
{
    
    public class ModelValidationFilterAttribute:FilterAttribute,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var response = filterContext.RequestContext.HttpContext.Response;
                response.StatusCode = (int) HttpStatusCode.BadRequest;
              
                filterContext.Result = new JsonResult()
                {
                    RecursionLimit = 1,
                    ContentType = "application/json",
                    Data = JsonConvert.SerializeObject(
                    filterContext.Controller.ViewData.ModelState.Select(s=>s.Value.Errors), settings),
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}