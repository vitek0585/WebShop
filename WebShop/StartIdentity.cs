using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using WebShop.Core.Settings.Autofac;
using WebShop.Identity.StartUp;

[assembly: OwinStartup(typeof(WebShop.StartIdentity))]

namespace WebShop
{
    public class StartIdentity
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new StartUpApp();
            config.ConfigureAuth(app, "/Account/Login");
            var scope = AutofacBuilder.GetScope(app);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(scope));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(scope);


        }
    }
}
