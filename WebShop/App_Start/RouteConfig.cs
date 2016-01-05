using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebShop.Repo.Interfaces;

namespace WebShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("*.html|js|css|gif|jpg|jpeg|png|swf");
            routes.MapMvcAttributeRoutes();
            //routes.Add("handler",
            //    new Route("{handler}/{id}",null,new RouteValueDictionary()
            //    {
            //        {"handler",@"^getPhoto.*"}

            //    }, new HandlerProvider()));
           
           // routes.IgnoreRoute("getPhoto/{*path}");
            //routes.MapRoute(
            //    name: "CommonRoute",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
         
            
        }
    }

    
}
