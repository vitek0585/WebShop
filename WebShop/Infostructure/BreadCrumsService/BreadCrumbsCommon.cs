using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Routing;
using AutoMapper;
using WebShop.EFModel.Model;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Infostructure.BreadCrumsService
{

    public class BreadCrumbsCommon : BreadCrumbsBase<BreadCrumbsModel>
    {

        public override IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang, params string[] links)
        {
            var list = MatchRoute(url, links.Select(l => new BreadCrumbsModel { NameLink = l }));
            return list;
        }

        protected override IBreadCrumbsModel CreateLink(UriBuilder uriBuilder, IEnumerable<BreadCrumbsModel> link, RouteData route, int i)
        {
            link.ElementAt(i).Href = uriBuilder.ToString();
        
            return link.ElementAt(i);
        }
    }
}