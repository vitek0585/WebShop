using System.Web.Mvc;

namespace WebShop.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.
            //context.MapRoute(
            //    "AdminRoute",
            //    "Admin/{action}/{id}",
            //    new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}