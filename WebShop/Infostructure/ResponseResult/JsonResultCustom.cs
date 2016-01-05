using System.Net;
using System.Web.Mvc;

namespace WebShop.Infostructure.ResponseResult
{
    public class JsonResultCustom : JsonResult
    {
        private HttpStatusCode _status;
        public JsonResultCustom(object data, HttpStatusCode status)
        {
            _status = status;
            Data = data;
        }
        public JsonResultCustom(object data)
        {
            _status = HttpStatusCode.OK;
            Data = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int) _status;
            response.Write(Data);
        }

    }
}