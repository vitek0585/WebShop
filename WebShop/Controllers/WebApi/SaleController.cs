using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using WebShop.Repo.Interfaces;

namespace WebShop.Controllers.WebApi
{
    [RoutePrefix("api/Sale")]
    public class SaleController : ApiController
    {
        private ISaleRepository _repository;

        public SaleController(ISaleRepository repository)
        {
            _repository = repository;
        }
        [HttpPost]
        [Route("GetHistory")]
        public IHttpActionResult GetHistory([FromBody]string userName)
        {
            var list = _repository.FindBy(s => s.UserName == userName).Select(i => new
            {
                count = i.SalePos.Count,
                date = i.DateSale,
                //summa = i.Summa,
                orders = i.SalePos.Select(p => p.ClassificationGood.Good.GoodNameRu)
            }).ToList();

            return Ok(JsonConvert.SerializeObject(list));
        }

    }
}
