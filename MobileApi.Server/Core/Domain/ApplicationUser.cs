using Microsoft.AspNet.Identity.EntityFramework;

namespace MobileApi.Server.Core.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public virtual UserProfile Profile { get; set; }
    }
}