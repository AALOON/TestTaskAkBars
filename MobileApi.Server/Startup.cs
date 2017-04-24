using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using MobileApi.Server.Core;
using MobileApi.Server.Infrastructure;
using MobileApi.Server.Providers;
using Owin;

namespace MobileApi.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true, //TODO: remove it after SSL certificates is installed.
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(),
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        public static IUnitOfWork GetUnitOfWorkInstance()
        {
            return new UnitOfWork(ApplicationDbContext.Create());
        }

        public static IUnitOfWork GetUnitOfWorkInstance(ApplicationDbContext dbContext)
        {
            return new UnitOfWork(dbContext);
        }
    }
}
