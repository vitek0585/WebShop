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
    public abstract class BreadCrumbsBase<TElement>
    {
        public abstract IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang,
            params string[] links);

        protected abstract IBreadCrumbsModel CreateLink(UriBuilder uriBuilder, IEnumerable<TElement> link, RouteData route, int i);

        protected IEnumerable<IBreadCrumbsModel> MatchRoute(Uri url, IEnumerable<TElement> links)
        {
            var list = new List<IBreadCrumbsModel>();
            try
            {
                var path  = url.Segments.Skip(1).ToArray();
                var uriBuilder = new UriBuilder(url.Scheme, url.Host, url.Port);

                for (int i = 0, pcount = 0; i < links.Count(); pcount++)
                {
                    GetSegmentSafety(uriBuilder, path, pcount);
                    var route = GetRouteDataByUrl(uriBuilder);

                    if (route != null)
                    {
                        list.Add(CreateLink(uriBuilder, links, route, i));
                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return list;
        }

        #region Additional Methods

        protected void GetSegmentSafety(UriBuilder uriBuilder, string[] path, int i)
        {
            if (i < path.Length)
            {
                uriBuilder.Path += path[i].TrimStart('/');
            }
        }

        protected RouteData GetRouteDataByUrl(UriBuilder uriBuilder)
        {
            var hc = new HttpContext(new HttpRequest(String.Empty, uriBuilder.Uri.AbsoluteUri, string.Empty),
                new HttpResponse(null));
            var route = RouteTable.Routes.GetRouteData(new HttpContextWrapper(hc));
            return route;
        }

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