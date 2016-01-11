using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using WebShop.EFModel.Model;

namespace WebShop.Infostructure.Binder
{
    public class HttpFilterBinder : IModelBinder
    {
        private ConditionalGeneratorSimple<Good> _generator;

        public HttpFilterBinder()
        {

        }
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var values = actionContext.Request.RequestUri.ParseQueryString();
            _generator = new ConditionalGeneratorSimple<Good>(values);

            _generator.SetKeyValueExpression<IEnumerable<int>, IEnumerable<int>>
                ((g, c, s) => g.ClassificationGoods.Any(cat => c.Contains(cat.ColorId) && s.Contains(cat.SizeId)), "colors", "sizes");
            _generator.SetKeyValueExpression<IEnumerable<int>>((g, c) => g.ClassificationGoods.Any(cat => c.Contains(cat.ColorId)), false, "colors");
            _generator.SetKeyValueExpression<IEnumerable<int>>((g, s) => g.ClassificationGoods.Any(cat => s.Contains(cat.SizeId)), false, "sizes");

            bindingContext.Model = _generator.GetConditional();
     
            return true;
        }
    }
}