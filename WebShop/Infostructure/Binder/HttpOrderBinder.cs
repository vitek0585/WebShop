using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using WebShop.Api.FilterBinder;
using WebShop.EFModel.Model;

namespace WebShop.Infostructure.Binder
{
    public class HttpOrderBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            try
            {
              
                var conditional = new ConditionalOrder<Good>(actionContext.Request.RequestUri.ParseQueryString(), "sort");
                conditional.SetKeyValueExpression(g => g.GoodId, "none");
                conditional.SetKeyValueExpression(g => g.DateCreate, "date", Order.Desc);
                conditional.SetKeyValueExpression("PriceWithDiscount", "priceAsc");
                conditional.SetKeyValueExpression("PriceWithDiscount", "priceDesc", Order.Desc);

                bindingContext.Model = conditional.GetConditional();
                return true;
            }
            catch (Exception e)
            {
                bindingContext.Model = e.Message;
                Console.WriteLine(e);
                return true;

            }
        }
    }
}