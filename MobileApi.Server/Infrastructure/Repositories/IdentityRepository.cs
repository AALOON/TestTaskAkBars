using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MobileApi.Server.Core;
using MobileApi.Server.Core.Domain;
using MobileApi.Server.Core.Repositories;
using MobileApi.Server.Models;
using MobileApi.Server.Providers;
using NLog;

namespace MobileApi.Server.Infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository, IDisposable
    {
        private static readonly string SmsMessage = ConfigurationManager.AppSettings[nameof(SmsMessage)];

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ApplicationUserManager _userManager;

        public IdentityRepository()
        {
            _userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public async Task<IIdentityResult> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerViewModel.PhoneNumber,
                PhoneNumber = registerViewModel.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user.Id, user.PhoneNumber);      

                var message = new IdentityMessage
                {
                    Destination = registerViewModel.PhoneNumber,
                    Body = string.Format(SmsMessage, code)
                };
                
                Logger.Info("Sms message Code is generated: {0}", code);

                await _userManager.SmsService.SendAsync(message);
            }

            return new ApplicationIdentityResult(result);
        }

        public async Task<IIdentityResult> VerifyPhoneNumberAsync(VerifyCodeViewModel verifyCodeViewModel)
        {
            var user = await GetCurrentApplicationUserAsync();

            if(user.PhoneNumberConfirmed)
                return new ApplicationIdentityResult(IdentityResult.Failed("Phone Number Already Confirmed."));

            bool verification = await _userManager.VerifyChangePhoneNumberTokenAsync(user.Id, verifyCodeViewModel.Code, user.PhoneNumber);
            
            if (verification)
            {
                user.PhoneNumberConfirmed = true;
                await _userManager.UpdateAsync(user);

                return new ApplicationIdentityResult(IdentityResult.Success);
            }

            return new ApplicationIdentityResult(IdentityResult.Failed());
        }

        public async Task<ApplicationUser> FindUserAsync(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<ApplicationUser> FindUserByUserNameAsync(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            return user;
        }

        public async Task<ApplicationUser> GetCurrentApplicationUserAsync()
        {
            var claim = HttpContext.Current.GetOwinContext().Authentication.User.Claims.FirstOrDefault();
            if (claim == null)
                return null;
            return await FindUserByUserNameAsync(claim.Value);
        }

        #region < IDisposable >

        public void Dispose()
        {
            _userManager.Dispose();
        }

        #endregion
    }
}