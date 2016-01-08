using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using WebShop.EFModel.Model;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Infostructure.BreadCrumsService
{
    public class BreadCrumbsCategory : BreadCrumbsBase<IBreadCrumbsCategoryModel>
    {
        private ICategoryRepository _categoryRepository;

        private int _categoryId;
        public BreadCrumbsCategory(int id)
        {
            _categoryId = id;
            _categoryRepository = DependencyResolver.Current.GetService<ICategoryRepository>();
        }

        private IEnumerable<IBreadCrumbsCategoryModel> CategoryWithChild(int id, string lang, params string[] links)
        {
            var data = _categoryRepository.EnableProxy<ICategoryRepository>().GetAll()
                .Where(c => c.CategoryId == id).Include(c => c.Parent).Include(c => c.Name)
                .Include(c => c.Parent.Name).Include(c => c.Parent.Parent)
                .FirstOrDefault();

            if (data == null)
                throw new ArgumentException(string.Format("Category by id {0} not found", id));

            var result = CategoryAndChild(data).Select(c => new
            {
                c.CategoryId,
                NameLink =
                    GetPropertyName<CategoryName>(lang, cn => cn.CategoryNameRu).GetValue(c.Name),
            });
            var linksMutable = links.Select(l => new { NameLink = l, CategoryId = 0 })
                .Concat<dynamic>(result.Reverse()).Select(l => new { l.NameLink, l.CategoryId });

            return AutoMapper.Mapper.DynamicMap<IEnumerable<IBreadCrumbsCategoryModel>>(linksMutable);

        }

        private IEnumerable<Category> CategoryAndChild(Category data)
        {
            var source = data;
            do
            {
                yield return source;
                source = source.Parent;

            } while (source != null);
        }

        public override IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang, params string[] links)
        {
            var newLinks = CategoryWithChild(_categoryId, lang, links);
            return MatchRoute(url, newLinks);
        }

        protected override IBreadCrumbsModel CreateLink(UriBuilder uriBuilder, IEnumerable<IBreadCrumbsCategoryModel> link,
            RouteData route, int i)
        {
            var model = new BreadCrumbsModel();
            model.NameLink = link.ElementAt(i).NameLink;
            var list = ((List<RouteData>)route.Values["MS_DirectRouteMatches"])[0].Values;

            if (string.Equals(list["controller"].ToString(), "Catalog", StringComparison.OrdinalIgnoreCase)
                && string.Equals(list["action"].ToString(), "CategoryGoods", StringComparison.OrdinalIgnoreCase))
            {
                var s = uriBuilder.Uri.Segments.Take(uriBuilder.Uri.Segments.Length - 1);
                uriBuilder.Path = string.Join("", s) + link.ElementAt(i).CategoryId.ToString().TrimStart('/');
                model.Href = uriBuilder.ToString();

            }
            else
                model.Href = uriBuilder.ToString();

            return model;
        }

    }
}