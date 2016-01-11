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

    [RoutePrefix("Category")]
    [TypeOfCulture]
    public class GoodController : ShopBaseController
    {
        private ICategoryService _categoryService;
        private IGoodService _goodService;
        private short _sizeStorageViewed;

        public GoodController(IGoodService goodService, ICategoryService categoryService, ICookieConsumer storage)
            : base(storage)
        {
            _goodService = goodService;
            _categoryService = categoryService;
            _sizeStorageViewed = 5;
        }
     
        //[Route("{type:string}/Name{id:int}")]
        //public ActionResult CategoryByType(string type, int id)
        //{
        //    var data = _categoryService.GetCategoryByType<IExtendModel>(type, GetCurrentLanguage());

        //    return View(data);
        //}
        //[Route("{gender:int}/Details/{id:int}")]
        //public ActionResult GetDetails(int id)
        //{
        //    var rv = GetRecentlyViewed(id);
        //    var data = _goodService.GetOrdersById(rv.GetAll(), GetCurrentCurrency(), GetCurrentLanguage());
        //    TempData[ValuesProvider.RecentlyViewed] = data;
        //    return View(id);
        //}
        [Route("AllCategory")]
        public ActionResult AllCategory()
        {

            return Json(_categoryService.GetCategoryByTypeSale<dynamic>(1, GetCurrentLanguage(), 50), JsonRequestBehavior.AllowGet);
        }

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