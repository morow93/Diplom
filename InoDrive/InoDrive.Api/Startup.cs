using InoDrive.Api.App_Start;
using InoDrive.Api.Identity;
using InoDrive.Api.Providers;
using InoDrive.Domain.Repositories.Abstract;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using InoDrive.Domain.Contexts;
using System.Data.Entity;

[assembly: OwinStartup(typeof(InoDrive.Api.Startup))]
namespace InoDrive.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureOAuth(app, config);
            WebApiConfig.Register(config);
            UnityConfig.Register(config, app.GetDataProtectionProvider());
            AutoMapperConfig.Register();
            app.UseWebApi(config);

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, InoDrive.Domain.MigrationsAuthContext.Configuration>());
        }

        public void ConfigureOAuth(IAppBuilder app, HttpConfiguration config)
        {
            Func<IAuthenticationRepository> authenticationRepositoryFactory = () =>
                            (IAuthenticationRepository)config.DependencyResolver.GetService(typeof(IAuthenticationRepository));

            Func<ApplicationUserManager> applicationUserManagerFactory = () =>
                (ApplicationUserManager)config.DependencyResolver.GetService(typeof(ApplicationUserManager));

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5.0d),//auth time here
                Provider = new SimpleAuthorizationServerProvider(authenticationRepositoryFactory, applicationUserManagerFactory),
                RefreshTokenProvider = new SimpleRefreshTokenProvider(authenticationRepositoryFactory)
            };

            //Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}