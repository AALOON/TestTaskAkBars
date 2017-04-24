using MobileApi.Server.Core.Domain;

namespace MobileApi.Server.Core.Repositories
{
    public interface IUserProfileRepository : IRepository<UserProfile, string>
    {
        bool IsExists(string id);
    }
}
