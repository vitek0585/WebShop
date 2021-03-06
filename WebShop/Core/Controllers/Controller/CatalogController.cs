﻿using System.Collections.Generic;
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
    using ICatModel = ITypeCategoryModel<ICategoryModel>;

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
        public ActionResult Categories(string type)
        {
            var discount = 0;
            var id = _categoryService.GetTypeIdByName(type);

            if (id.HasValue)
            {
                var data = _categoryService.GetCategoryByType<ICatModel>(id.Value, GetCurrentLanguage());
                ViewBag.Sale = _categoryService.GetCategoryByTypeSale<ICatModel>(id.Value, GetCurrentLanguage(), discount);
                return View(data);
            }

            return HttpNotFound();

        }
        [Route("{type}/Category/{id:int:min(1):max(100000)}/{page:int:min(1):max(1000)=1}")]
        public ActionResult CategoryGoods(string type, int id, int page)
        {
            var data = _categoryService.GetCategoryByCulture<ICategoryCulture>(type, id, GetCurrentLanguage());

            if (data != null)
            {
                ViewBag.Page = page;
                //ViewBag.BreadCrums = _categoryService.GetCategoryWithChild<IBreadCrumbsModel>(id, GetCurrentLanguage());
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
            //var data = _categoryService.GetCategoryWithChild<IBreadCrumbsModel>(51, GetCurrentLanguage());
             var d = _categoryService.GetCategoryByTypeSale<ICatModel>(1, GetCurrentLanguage(), 0);
           //var d = _categoryService.GetCategoryByCulture<ICategoryCulture>("Men", 42, GetCurrentLanguage());
            return Json(d, JsonRequestBehavior.AllowGet);
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