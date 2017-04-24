using System.Threading.Tasks;
using MobileApi.Server.Core.Domain;
using MobileApi.Server.Models;

namespace MobileApi.Server.Core.Repositories
{
    public interface IIdentityRepository
    {
        Task<IIdentityResult> RegisterUserAsync(RegisterViewModel registerViewModel);

        Task<ApplicationUser> FindUserAsync(string userName, string password);

        Task<ApplicationUser> FindUserByUserNameAsync(string userName);

        Task<IIdentityResult> VerifyPhoneNumberAsync(VerifyCodeViewModel verifyCodeViewModel);

        Task<ApplicationUser> GetCurrentApplicationUserAsync();
    }
}