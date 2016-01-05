using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebShop.Filters.ModelValidate
{
    public class ModelValidatorWebApiAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                         HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }
}