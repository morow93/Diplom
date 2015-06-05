using InoDrive.Api.Resolver;
using InoDrive.Domain.Contexts;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Repositories.Abstract;
using InoDrive.Domain.Repositories.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace InoDrive.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config, IDataProtectionProvider dataProtectionProvider)
        {
            var container = new UnityContainer();

            //container.RegisterType<AuthContext>();
            //container.RegisterType<DataContext>();

            //container.RegisterType<IPostRepository, PostRepository>(
            //    new HierarchicalLifetimeManager(), new InjectionConstructor(typeof(DataContext)));

            container.RegisterType<IAuthenticationRepository, AuthenticationRepository>(
                new HierarchicalLifetimeManager(), new InjectionConstructor(typeof(AuthContext)));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new InjectionConstructor(typeof(AuthContext)));

            container.RegisterInstance(dataProtectionProvider);

            config.DependencyResolver = new UnityResolver(container);
        }
    }
}