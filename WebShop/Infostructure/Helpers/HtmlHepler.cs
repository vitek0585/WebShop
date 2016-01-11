using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using WebShop.App_GlobalResources;
using WebShop.Infostructure.BreadCrumsService;

namespace WebShop.Infostructure.Helpers
{
    public static class HtmlExtensionsBreadCrumbs
    {
        public static MvcHtmlString GenerateBreadCrumbs<T>(this HtmlHelper html, BreadCrumbsBase<T> crumbs, params string[] links)
        {
            TagBuilder ol = new TagBuilder("ol");
            ol.AddCssClass("breadcrumb");
            

            try
            {
                var lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                Uri url = html.ViewContext.HttpContext.Request.Url;
                var data = crumbs.GenerateBreadCrumbs(url, lang, links);

                TagBuilder li = new TagBuilder("li");
                li.InnerHtml += CreateTagLink(Resource.Main, new UriBuilder(url.Scheme,url.Host,url.Port).ToString());
                ol.InnerHtml += li.ToString();
                for (var i = 0; i < data.Count(); i++)
                {
                
                    li = new TagBuilder("li");
                    if (i == data.Count() - 1)
                    {
                        li.AddCssClass("active");
                        TagBuilder tb = new TagBuilder("span");
                        tb.SetInnerText(data.ElementAt(i).NameLink);
                        li.InnerHtml += tb.ToString();
                    }else
                    li.InnerHtml += CreateTagLink(data.ElementAt(i).NameLink, data.ElementAt(i).Href);

                    ol.InnerHtml += li.ToString();
                }
            }
            catch (Exception e)
            {
                return MvcHtmlString.Create(e.Message + String.Empty);
            }
            return MvcHtmlString.Create(ol.ToString());
        }

        private static string CreateTagLink(string text,string href)
        {
            TagBuilder tb = new TagBuilder("a");
            tb.MergeAttribute("href", href);
            tb.SetInnerText(text.ToUpper());
            return tb.ToString();
        }
    }
}