using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using AutoMapper;
using WebShop.EFModel.Model;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Infostructure.BreadCrumsService
{
    public abstract class BreadCrumbsBase
    {
        public abstract IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang,
            params string[] links);

        
        #region Additional Methods


        protected PropertyInfo GetPropertyName<TItem>(string lang, Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var name = GetNameLamdaBody(lang, expression);
            var property = typeof(TItem).GetProperty(name,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return property;
        }
        private string GetNameLamdaBody<TItem>(string lang, Expression<Func<TItem, string>> expression)
            where TItem : class
        {
            var exp = (MemberExpression)expression.Body;
            var str = exp.Member.Name;
            return string.Concat(str.Substring(0, str.Length - 2), lang);
        }
        #endregion

    }
}