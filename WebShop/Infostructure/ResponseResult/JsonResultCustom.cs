using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebShop.Infostructure.ResponseResult
{
    public class JsonResultCustom : JsonResult
    {
        private HttpStatusCode _status;
        public JsonResultCustom(object data, HttpStatusCode status)
        {
            _status = status;
            Data = JsonConvert.SerializeObject(data);
        }
        public JsonResultCustom(object data)
        {
            _status = HttpStatusCode.OK;
            Data = data;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            
            var response = context.HttpContext.Response;
            response.StatusCode = (int) _status;
            response.Output.Write(Data);
        }

    }
}