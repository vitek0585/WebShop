using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using WebShop.EFModel.Model;
using WebShop.Models;

namespace WebShop.Filters
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false)]
    public class ConvertToAllInfoAttribute:FilterAttribute,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var goods = (IEnumerable<Good>)(filterContext.Result as ViewResult).Model;
                var order = Mapper.Map<IEnumerable<UserOrder>>(goods);
                filterContext.Controller.ViewData.Model = order;
            }
            catch (Exception e)
            {
                filterContext.Exception = new Exception("Error convert");
                
            }
        }
    }
}