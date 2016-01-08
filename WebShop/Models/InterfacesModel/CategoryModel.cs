using System;
using System.Collections.Generic;

namespace WebShop.Models.InterfacesModel
{
    public interface IBreadCrumbsModel
    {
        string NameLink { get; set; }
        string Href { get; set; }
       
    }

    public class BreadCrumbsModel:IBreadCrumbsModel
    {
        public string NameLink { get; set; }
        public string Href { get; set; }


    }

    public interface IBreadCrumbsCategoryModel : IBreadCrumbsModel
    {
        int CategoryId { get; set; }

    }

    public interface ICategoryCulture
    {
        int CategoryId { get; set; }
        string TypeName { get; set; }
        string CategoryName { get; set; }
        string TypeByHref { get; set; }


    }
    public interface ITypeCategoryModel<T> where T : ICategoryModelBase
    {
        int TypeId { get; set; }
        string TypeName { get; set; }
        string TypeByHref { get; set; }
        IEnumerable<T> Items { get; set; }
    }
    public interface ICategoryModelBase
    {
        int CategoryId { get; set; }
        string CategoryName { get; set; }

    }
    public interface ICategoryModel : ICategoryModelBase
    {
        int Discount { get; set; }
        string Photo { get; set; }
    }
}