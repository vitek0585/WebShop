using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebShop.Infostructure.Helpers
{
    public static class HtmlHeplerAj
    {
        public static string GetName<TModel, TResult>(this HtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression)
        {
            try
            {
                MemberExpression member = expression.Body as MemberExpression;
                return member.Member.Name;

            }
            catch (Exception)
            {
                return String.Empty;
            }
            
        }
    }
}