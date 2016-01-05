using System.Web.Http;
using WebShop.Identity.Manager;

namespace WebShop.Areas.Administration.Controllers.WebApi
{
    public class AdminApiController : ApiController
    {
        private UserManager _userManager;
        
        public AdminApiController(UserManager userManager)
        {
            _userManager = userManager;
        }

        
    }
}
