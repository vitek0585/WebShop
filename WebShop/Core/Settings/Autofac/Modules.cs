using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using WebShop.Api.CalculatePrice;
using WebShop.Api.Storage;
using WebShop.App_GlobalResources;
using WebShop.EFModel.Context;
using WebShop.EFModel.Model;
using WebShop.Identity.Context;
using WebShop.Identity.Interfaces;
using WebShop.Identity.Manager;
using WebShop.Identity.Manager.CustomValidation;
using WebShop.Identity.Models;
using WebShop.Identity.Services;
using WebShop.Infostructure.Cart;
using WebShop.Infostructure.Storage.Implements;
using WebShop.Infostructure.Storage.Interfaces;
using WebShop.Models;
using WebShop.Repo.Interfaces;
using WebShop.Repo.Repositories;
using WebShop.Repo.Services;

namespace WebShop.Core.Settings.Autofac
{
    public class RepositoryModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(GoodsRepository)).As(typeof(IGoodsRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CategoryRepository)).As(typeof(ICategoryRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CategoryTypeRepository)).As(typeof(ICategoryTypeRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(SaleRepository)).As(typeof(ISaleRepository)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(SalePosRepository)).As(typeof(ISalePosRepository)).InstancePerLifetimeScope();
            builder.RegisterType<PhotoGoodsRepository>().As<IPhotoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ClassificationGoodRepository>().As<IClassificationGoodRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ColorRepository>().As<IColorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SizeRepository>().As<ISizeRepository>().InstancePerLifetimeScope();


            builder.RegisterType<ExchangeRatesRepository>().As<IExchangeRatesRepository>().InstancePerLifetimeScope();

        }
    }
    public class ServicesModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(GoodService)).As(typeof(IGoodService)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CategoryService)).As(typeof(ICategoryService)).InstancePerLifetimeScope();

            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.privatbank.ua";
            uriBuilder.Path = "p24api/pubinfo";
            uriBuilder.Query = "json&exchange&coursid=5";

            builder.Register(i => new ExchangeRatesService(DependencyResolver.Current.GetService<IUnitOfWork>(),
                DependencyResolver.Current.GetService<IExchangeRatesRepository>(), uriBuilder.ToString()))
                .As<IExchangeRatesService>().InstancePerLifetimeScope();
        }
    }

    public class UnitModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
    public class OtherModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CookieConsumer>().As<ICookieConsumer>().InstancePerLifetimeScope();
            builder.RegisterType<CartProvider>().As<ICartProvider<UserOrder>>().InstancePerLifetimeScope();


        }
    }
    public class ContextModule : global::Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(ShopContext)).AsSelf().InstancePerRequest();//.OnPreparing((args) => { args.Parameters. })
        }

    }
    public class IdentityModule : global::Autofac.Module
    {
        private IAppBuilder _app;

        public IdentityModule(IAppBuilder app)
        {
            _app = app;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(AccountService)).As(typeof(IAccountService)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(DbContextIdentity)).AsSelf().InstancePerRequest();
            builder.RegisterType<DbContextIdentity>().As<DbContext>().InstancePerRequest();

            builder.RegisterType<UserStore<User, Role, int, UserExternLogin, UserRole, Claim>>().
                As<IUserStore<User, int>>().InstancePerRequest();
            builder.RegisterType<RoleStore<Role, int, UserRole>>().As<IRoleStore<Role, int>>().InstancePerRequest();

            builder.RegisterType<UserManager>().AsSelf().InstancePerRequest();

            builder.RegisterType<RoleManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<SignInManager>().AsSelf().InstancePerRequest();
            builder.Register(i => new IdentityErrors()
            {
                NameInValid = Resource.NameInValid,
                NameDuplicat = Resource.NameDuplicat,
                EmailInValid = Resource.EmailInValid,
                EmailDuplicat = Resource.EmailDuplicat

            }).As<IIdentityErrros>().InstancePerRequest();

            builder.RegisterType<UnitOfWorkIdentity>().As<IUnitOfWorkIdentity>().InstancePerRequest();

            builder.Register<IDataProtectionProvider>(c => _app.GetDataProtectionProvider()).InstancePerRequest();
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
        }
    }
}