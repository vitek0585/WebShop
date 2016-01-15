using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using WebShop.EFModel.Model;
using WebShop.Models.InterfacesModel;
using WebShop.Repo.Interfaces;

namespace WebShop.Infostructure.BreadCrumsService
{

    public class BreadCrumbsCommon : BreadCrumbsBase
    {
        private UrlHelper _urlHelper;

        public BreadCrumbsCommon(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public override IEnumerable<IBreadCrumbsModel> GenerateBreadCrumbs(Uri url, string lang, params string[] links)
        {
            
            yield return new BreadCrumbsModel()
            {
                Href = _urlHelper.Action("Categories","Catalog",new {type = url.Segments[1]}),
                NameLink = links[0]
            };
        }

        
    }
}