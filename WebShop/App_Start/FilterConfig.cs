using System.Web.Mvc;
using WebShop.Filters.Headers;

namespace WebShop
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HeaderDataProvider(), 0);
        }
    }
}
