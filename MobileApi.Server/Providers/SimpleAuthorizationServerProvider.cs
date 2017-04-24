using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;

namespace MobileApi.Server.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            
            using (var unitOfWork = Startup.GetUnitOfWorkInstance())
            {
                IdentityUser user = await unitOfWork.Identity.FindUserAsync(context.UserName, context.Password);
                
                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));

                if(user.PhoneNumberConfirmed)
                    identity.AddClaim(new Claim("role", "verified-user"));
                else
                    identity.AddClaim(new Claim("role", "user"));
                
                context.Validated(identity);
            }
        }
    }
}
