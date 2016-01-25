using System.Web.Mvc;
using WebShop.Core.Controllers.Base;
using WebShop.Filters.Headers;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Core.Controllers.Controller
{
    [RoutePrefix("Fashion")]
  
    public class GoodController : ShopBaseController
    {
        private IGoodService _goodService;
        private short _sizeStorageViewed;

        public GoodController(IGoodService goodService,ICookieConsumer storage)
            : base(storage)
        {
            _goodService = goodService;
            _sizeStorageViewed = 20;
        }
        [Route("Good/{id:min(1):max(10000000)}")]
        public ActionResult GetDetails(int id)
        {
            RecentlyViewed(id);
            var data = _goodService.GetGood<IGoodModel>(id, GetCurrentCurrency(), GetCurrentLanguage());
            
            return View(data);
        }

        #region Child action

        [ChildActionOnly, Route("RecentlyViewedUser")]
        public ActionResult RecentlyViewedUser()
        {
            var ids = RecentlyViewed(null).GetAll();
            var data = _goodService.GetGoods<IGoodModel>(ids, GetCurrentLanguage());
            return PartialView(data);

        }

        #endregion
        #region Helper
        [NonAction]
        protected RecentlyViewedStorage RecentlyViewed(int? id)
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