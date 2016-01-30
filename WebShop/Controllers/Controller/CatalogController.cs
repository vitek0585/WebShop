using System.Linq;
using System.Web.Mvc;
using WebShop.Controllers.Base;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.Controller
{
    using ICatModel = ITypeCategoryModel<ICategoryModel>;

    [RoutePrefix("Catalog")]
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
        public ActionResult Categories(string type)
        {
            var data = _categoryService.GetCategoriesByType<ICatModel>(type, GetCurrentLanguage());

            if (data != null)
            {
                return View(data);
            }

            return HttpNotFound();

        }
        [Route("{type}/Category/{id:int:min(1):max(100000)}/{page:int:min(1):max(1000)=1}")]
        public ActionResult Category(string type, int id, int page)
        {
            var data = _categoryService.GetCategoryByCulture<ICategoryDescModel>(type, id, GetCurrentLanguage());

            if (data != null)
            {
                ViewBag.Page = page;
                return View(data);
            }

            return HttpNotFound();

        }

        [Route("AllCategory")]
        public JsonResult AllCategory()
        {
            //var data = _goodService.GetByPage<dynamic>(1, _totalPerPage,10, GetCurrentCurrency(), GetCurrentLanguage());
            //var data = _categoryService.GetCategoryWithChild<IBreadCrumbsModel>(51, GetCurrentLanguage());
            //var d = _categoryService.GetCategoryByTypeSale<ICatModel>(1, GetCurrentLanguage(), 0);

            var data = _goodService.GetGoods<IGoodModel>(Enumerable.Empty<int>(), GetCurrentLanguage());
            //var d = _categoryService.GetInformationAboutCategory<IFilterModel>(40, GetCurrentCurrency(), GetCurrentLanguage());
            //var d = _categoryService.GetCategoryByCulture<ICategoryCulture>("Men", 42, GetCurrentLanguage());
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Child action

        [ChildActionOnly, Route("Filter")]
        public ActionResult Filter(int id)
        {
            var data = _categoryService.GetInformationAboutCategory<IFilterModel>(id, GetCurrentCurrency(),
                GetCurrentLanguage());
            return PartialView(data);
        }

        [ChildActionOnly, Route("Sale/{type}/{discount:min(0):max(100):int?}")]
        public ActionResult Sale(string type, int discount = 50)
        {
            var data = _categoryService.GetCategoriesSale<ICatModel>(type, GetCurrentLanguage(), discount);

            return PartialView(data);
        }

        #endregion

        #region Helper
        [NonAction]
        private RecentlyViewedStorage GetRecentlyViewed(int? id)
        {
            var viewed = (RecentlyViewedStorage)Session[ValuesApp.RecentlyViewed];
            if (viewed == null)
            {
                Session[ValuesApp.RecentlyViewed] = viewed = new RecentlyViewedStorage(_sizeStorageViewed);
            }

            if (id.HasValue)
                viewed.Add(id.Value);

            return viewed;
        }

        #endregion
    }
}