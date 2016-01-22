using System.Web.Mvc;
using WebShop.Core.Controllers.Base;
using WebShop.Filters.Culture;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.Controller
{
   
    [RoutePrefix("Main")]
    public class MainController : ShopBaseController
    {
        private IGoodService _goodService;
        private ICategoryService _categoryService; 


        public MainController(IGoodService goodService,ICategoryService category, ICookieConsumer storage) 
            : base(storage)
        {
            _categoryService = category;
            _goodService = goodService;
            
        }
        [Route("~/")]
        public ActionResult Index()
        {
            var randCount = 8;
            ViewBag.RandCategories = _categoryService.GetRandom<ICategoryModel>(randCount, GetCurrentLanguage());

            return View();
        }

        [Route("ChangeLanguage")]
        public ActionResult ChangeLanguage(string lang, string refUrl)
        {
            _storage.SetValueStorage(ControllerContext.HttpContext, ValuesApp.Language, 
                lang, ValuesApp.Languages);

            return Redirect(CheckValidReturnUrl(refUrl));
        }
        [Route("ChangeCurrency")]
        public ActionResult ChangeCurrency(string currency, string refUrl)
        {
            _storage.SetValueStorage(ControllerContext.HttpContext, ValuesApp.Currency, 
                currency, ValuesApp.Currencies);

            return Redirect(CheckValidReturnUrl(refUrl));
        }
      
       
    }
}