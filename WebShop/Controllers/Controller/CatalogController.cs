using System.Web.Mvc;
using WebShop.Controllers.Base;
using WebShop.Filters.Culture;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.Controller
{
    using IExtendModel = ITypeCategoryModel<ICategoryModel>;

    [RoutePrefix("Catalog")]
    [TypeOfCulture]
    public class CatalogController : ShopBaseController
    {
        private ICategoryService _categoryService;
        private IGoodService _goodService;
        private short _sizeStorageViewed;
        private byte _totalPerPage = 9;

        public CatalogController(IGoodService goodService, ICategoryService categoryService, ICookieConsumer storage)
            : base(storage)
        {
            _goodService = goodService;
            _categoryService = categoryService;
            _sizeStorageViewed = 5;
        }
        [Route("{type}")]
        public ActionResult CategoryByType(string type, int id)
        {
            var discount = 50;

            var data = _categoryService.GetCategoryByType<IExtendModel>(id, GetCurrentLanguage());
            ViewBag.Sale = _categoryService.GetCategoryByTypeSale<IExtendModel>(id, GetCurrentLanguage(), discount);

            return View(data);
        }
        [Route("{type}/Category/{id:int}/{page:int:min(1)=1}")]
        public ActionResult GetGoodsByCategory(string type, int id, int page)
        {
            var data = _categoryService.GetCategoryByCulture<ICategoryCulture>(type, id, GetCurrentLanguage());
            if (data != null)
            {
                ViewBag.Page = page;
                return View(data);
            }
            return HttpNotFound();

        }
        //[Route("{gender:int}/Details/{id:int}")]
        //public ActionResult GetDetails(int id)
        //{
        //    var rv = GetRecentlyViewed(id);
        //    var data = _goodService.GetOrdersById(rv.GetAll(), GetCurrentCurrency(), GetCurrentLanguage());
        //    TempData[ValuesProvider.RecentlyViewed] = data;
        //    return View(id);
        //}
        [Route("AllCategory")]
        public JsonResult AllCategory()
        {
            //var data = _goodService.GetByPage<dynamic>(1, _totalPerPage,10, GetCurrentCurrency(), GetCurrentLanguage());
            return Json(_categoryService.GetCategoryByCulture<ICategoryCulture>("Women",10, GetCurrentLanguage()), JsonRequestBehavior.AllowGet);
        }

        #region Helper
        [NonAction]
        private RecentlyViewedStorage GetRecentlyViewed(int? id)
        {
            var viewed = (RecentlyViewedStorage)Session[ValuesProvider.RecentlyViewed];
            if (viewed == null)
            {
                Session[ValuesProvider.RecentlyViewed] = viewed = new RecentlyViewedStorage(_sizeStorageViewed);
            }

            if (id.HasValue)
                viewed.Add(id.Value);

            return viewed;
        }

        #endregion
    }
}