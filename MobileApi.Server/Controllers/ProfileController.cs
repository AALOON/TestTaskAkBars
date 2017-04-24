using System;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using MobileApi.Server.Core;
using MobileApi.Server.Core.Domain;
using MobileApi.Server.Infrastructure;
using MobileApi.Server.Models;
using NLog;

namespace MobileApi.Server.Controllers
{
    [Authorize]
    [ClaimsAuthorization(ClaimType = "role", ClaimValue = "verified-user")]
    public class ProfileController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;

        public IIdentity CurrentIdentity => User.Identity;

        public ProfileController()
        {
            _unitOfWork = Startup.GetUnitOfWorkInstance();
        }

        public ProfileController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Profile
        public async Task<IHttpActionResult> Get()
        {
            var user = await _unitOfWork.Identity.GetCurrentApplicationUserAsync();
            var userProfile = _unitOfWork.UserProfile.Get(user.Id);
            if (userProfile != null)
            {
                var viewModel = GetViewModel(userProfile);
                return Ok(viewModel);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Profile   //Add
        public async Task<IHttpActionResult> Post([FromBody] ProfileViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _unitOfWork.Identity.GetCurrentApplicationUserAsync();

            if (_unitOfWork.UserProfile.IsExists(user.Id))
            {
                Logger.Warn("User tried to add new profile, while it is already exists.");
                return Conflict();
            }

            var userProfile = GetDataModel(viewModel);
            userProfile.UserId = user.Id;

            _unitOfWork.UserProfile.Add(userProfile);

            try
            {
                _unitOfWork.Save();

                Logger.Info("User {0} Created Profile.", user.Id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unabled to save data during the POST.");
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            
            return StatusCode(HttpStatusCode.Created);
        }

        // PUT: api/Profile   //Update
        public async Task<IHttpActionResult> Put([FromBody] ProfileViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _unitOfWork.Identity.GetCurrentApplicationUserAsync();

            var userProfile = _unitOfWork.UserProfile.Get(user.Id);

            if (userProfile == null)
            {
                Logger.Warn("User tried to update profile while it is not exists.");
                //Nothing to update
                return StatusCode(HttpStatusCode.NoContent);
            }

            userProfile.FirstName = viewModel.FirstName;
            userProfile.SecondName = viewModel.SecondName;
            userProfile.Patronym = viewModel.Patronym;
            userProfile.DateOfBirth = viewModel.DateOfBirth;
            userProfile.AvatarLink = viewModel.AvatarLink;
            userProfile.Email = viewModel.Email;

            try
            {
                int changes = _unitOfWork.Save();
                if (changes > 1)
                {
                    Logger.Info("User {0} updated Profile.", user.Id);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unabled to save data during the PUT.");
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        // DELETE: api/Profile
        public async Task<IHttpActionResult> Delete()
        {
            var user = await _unitOfWork.Identity.GetCurrentApplicationUserAsync();

            var userProfile = _unitOfWork.UserProfile.Get(user.Id);

            if (userProfile == null)
            {
                Logger.Warn("User tried to delete profile while it is not exists.");
                //Nothing to update
                return StatusCode(HttpStatusCode.NoContent);
            }

            _unitOfWork.UserProfile.Remove(userProfile);

            try
            {
                _unitOfWork.Save();
                Logger.Info("User {0} deleted Profile.", user.Id);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unabled to save data during the DELETE.");
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return Ok();
        }

        #region < Helpers >

        /// <summary>
        /// Maper which converts data model to view model
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        private static ProfileViewModel GetViewModel(UserProfile userProfile)
        {
            var viewModel = new ProfileViewModel()
            {
                FirstName = userProfile.FirstName,
                SecondName = userProfile.SecondName,
                Patronym = userProfile.Patronym,
                Email = userProfile.Email,
                AvatarLink = userProfile.AvatarLink,
                DateOfBirth = userProfile.DateOfBirth
            };
            return viewModel;
        }

        /// <summary>
        /// Maper which converts view model to data model
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private static UserProfile GetDataModel(ProfileViewModel viewModel)
        {
            var userProfile = new UserProfile()
            {
                FirstName = viewModel.FirstName,
                SecondName = viewModel.SecondName,
                Patronym = viewModel.Patronym,
                Email = viewModel.Email,
                AvatarLink = viewModel.AvatarLink,
                DateOfBirth = viewModel.DateOfBirth
            };
            return userProfile;
        }

        #endregion
    }
}