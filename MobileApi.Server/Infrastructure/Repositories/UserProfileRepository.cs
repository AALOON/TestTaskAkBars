using System.Data.Entity;
using System.Linq;
using MobileApi.Server.Core.Domain;
using MobileApi.Server.Core.Repositories;

namespace MobileApi.Server.Infrastructure.Repositories
{
    public class UserProfileRepository : Repository<UserProfile, string>, IUserProfileRepository
    {
        public UserProfileRepository(DbContext context) : base(context)
        {
        }

        public ApplicationDbContext ApplicationContext => Context as ApplicationDbContext;

        public bool IsExists(string id)
        {
            return ApplicationContext.UserProfiles.Any(o => o.UserId == id);
        }
    }
}
