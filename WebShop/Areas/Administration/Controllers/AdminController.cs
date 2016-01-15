using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebShop.EFModel.Model;
using WebShop.Identity.Manager;
using WebShop.Identity.Models;

using WebShop.Models;
using WebShop.Repo.Interfaces;

namespace WebShop.Areas.Administration.Controllers
{

    [Authorize(Roles = "Admin")]
    [RouteArea("Administration")]
    [RoutePrefix("")]
    public class AdminController : Controller
    {
        private IGoodsRepository _goods;
        private RoleManager _roleManager;
        private UserManager _userManager;
        private byte totalPerPage = 5;

        public AdminController(IGoodsRepository goods, UserManager manager, RoleManager role)
        {
            _roleManager = role;
            _userManager = manager;
            _goods = goods;
        }
        [Route("")]
        public ActionResult Index(short id = 1)
        {
            return View("Index", new PageInfo<Good>(_goods.GetAll().Count(), totalPerPage, id));
        }

        public JsonResult GetUsers(short id)
        {
            var roles = _roleManager.Roles.Select(r => new { r.Id, r.Name });
            return Json(new PageInfo<User>(_userManager.Users,
                (u) => new { u.UserName, u.Id, userRole = u.Roles.Select(r => r.RoleId), roles }, totalPerPage, id));
        }
        public async Task<HttpStatusCodeResult> RemoveUser(short id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
               // await _userManager.DeleteAsync(user);
                return new HttpStatusCodeResult(HttpStatusCode.Accepted,
                    string.Format("User by id {0} was been removed", user.Id));

            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                string.Format("User by id {0} was not found", id));

        }

        public async Task<HttpStatusCodeResult> SetRoles(int id, string[] roles)
        {
            User user;
            try
            {
                user = await _userManager.FindByIdAsync(id);
                if (user == null || roles == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "User or role not found");
                }
                var currentRole = await _userManager.GetRolesAsync(id);

                if (currentRole.Any())
                {
                    var toDelete = currentRole.Except(roles).ToArray();
                    var toAdd = roles.Except(currentRole).ToArray();
                    var result = await _userManager.RemoveFromRolesAsync(id, toDelete);
                    if (result.Errors.Any())
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "RemoveFromRolesAsync");
                 
                    result = await _userManager.AddToRolesAsync(id, toAdd);
                    if (result.Errors.Any())
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "RemoveFromRolesAsync");

                }
                else
                {
                    var result = await _userManager.AddToRolesAsync(id, roles);
                    if (result.Errors.Any())
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "AddToRolesAsync");
                }


            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);

            }
            return new HttpStatusCodeResult(HttpStatusCode.OK,
                string.Format("Roles in user by id {0} was be refreshed", id));
        }

    }


}