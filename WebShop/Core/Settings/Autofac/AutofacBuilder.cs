using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Owin;

namespace WebShop.Core.Settings.Autofac
{
    
    public class AutofacBuilder
    {
       
        public static ILifetimeScope GetScope(IAppBuilder app)
        {
            var build = new ContainerBuilder();
            Assembly assembly = typeof(MvcApplication).Assembly;

            build.RegisterControllers(assembly).PropertiesAutowired();
            build.RegisterApiControllers(assembly);

            build.RegisterModule(new RepositoryModule());
            build.RegisterModule(new ContextModule());
            build.RegisterModule(new UnitModule());
            build.RegisterModule(new IdentityModule(app));
            build.RegisterModule(new OtherModule());
            build.RegisterModule(new ServicesModule());

            var container = build.Build();
            app.UseAutofacMvc();
            app.UseAutofacMiddleware(container);

            return container;
        }

    }

}