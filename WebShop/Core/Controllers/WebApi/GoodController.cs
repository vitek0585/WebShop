using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Services.Configuration;
using WebShop.Api.FilterBinder;
using WebShop.EFModel.Model;
using WebShop.Filters.ModelValidate;
using WebShop.Infostructure.Binder;
using WebShop.Infostructure.Common;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.WebApi
{
    [RoutePrefix("api/Good")]
    public class GoodController : ApiController
    {
        private IPhotoRepository _photo;
        private IGoodService _goodService;

        private ICookieConsumer _storage;
        private byte _totalPerPage = 9;

        public GoodController(IPhotoRepository photo, ICookieConsumer storage, IGoodService goodService)
        {
            _storage = storage;
            _goodService = goodService;
            _photo = photo;

        }
        [HttpGet]
        [Route("GetGood")]
        public IHttpActionResult GetGood([FromUri]int id)
        {
            Task.Delay(1500).GetAwaiter().GetResult();

            try
            {
                var data = _goodService.GetGood<dynamic>(id, GetCurrentCurrency(), GetCurrentLanguage());
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }


        [HttpGet]
        [Route("RandomGood")]
        public IHttpActionResult RandomGood(int count = 4)
        {
            Task.Delay(1500).GetAwaiter().GetResult();

            try
            {
                var data = _goodService.GetRandomGoods<dynamic>(count, GetCurrentCurrency(), GetCurrentLanguage());
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        [HttpGet]
        [Route("NewGoods")]
        public IHttpActionResult NewGoods()
        {
            try
            {
                int newCount = 4;
                var data = _goodService.GetNewGoods<dynamic>(newCount, GetCurrentCurrency(), GetCurrentLanguage());

                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

        }
        [Route("GetByPage"), HttpGet]
        public IHttpActionResult GetByPage(short category, int page,
            [ModelBinder(typeof(HttpFilterBinder))]Expression<Func<Good, bool>> pr,
            [ModelBinder(typeof(HttpOrderBinder))]string sort)
        {
            Task.Delay(1500).GetAwaiter().GetResult();
            try
            {
                var data = _goodService.GetByPage<dynamic>(page, _totalPerPage, category,
                     GetCurrentCurrency(), GetCurrentLanguage(), pr, sort);

                return OkOrNoContent<dynamic>(data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route("RemoveGood")]
        public HttpResponseMessage RemoveGood([FromBody]int id)
        {
            try
            {
                //_goods.Delete(_goods.FindBy(p => p.GoodId == id).First());
                //_unit.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted,
                    string.Format("The good by id {0} was deleted successfuly", id));
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    string.Format("The good by id {0} was not deleted", id));
            }
        }
        [HttpPost]
        [ModelValidatorWebApi]
        [Route("Create")]
        public HttpResponseMessage Create(GoodsWebApi goodApi)
        {
            Good good = null;
            try
            {
                //good = _goods.Add(goodApi.GetGood());
                Parallel.ForEach(goodApi.Files, async f =>
                {
                    await _photo.Add(good.GoodId, new MemoryStream(f.Data), f.FileName, f.MimeType);
                });

                //_unit.Save();
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.Accepted,
                string.Format("The good by id {0} was added successfuly", good.GoodId));
        }
        [HttpPost]
        [ModelValidatorWebApi]
        [Route("Update")]
        public HttpResponseMessage Update(Good good)
        {
            try
            {
                //_goods.UpdateOnlyField(good, g => g.GoodCount, g => g.GoodNameRu, g => g.PriceUsd);
                //_unit.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted, good);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("The good by id {0} not update", good.GoodId));
            }
        }

        #region Helper methods

        [NonAction]
        private string GetCurrentCurrency()
        {
            return _storage.GetValueStorage(Request.Headers, ValuesApp.Currency)
                   ?? ValuesApp.CurrencyDefault;
        }
        [NonAction]
        private string GetCurrentLanguage()
        {
            return _storage.GetValueStorage(Request.Headers, ValuesApp.Language) ?? ValuesApp.LanguageDefault;
        }
        [NonAction]
        private IHttpActionResult OkOrNoContent<TResult>(TResult data)
        {
            if (data == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return Ok(data);
        }

        #endregion

    }
}
