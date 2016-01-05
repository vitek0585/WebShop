using System.Web.Http;
using WebShop.Core.Settings;
using WebShop.Infostructure.Formaters;
using WebShop.Models;

namespace WebShop
{

   public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            JsonConfiguration.Setup(config);
            config.Formatters.Add(new UploadMultipartMediaTypeFormatter<GoodsWebApi>());
            config.Formatters.Add(new UploadImageMediaTypeFormatter());

        }
    }
}
