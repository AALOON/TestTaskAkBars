using System.Threading.Tasks;
using System.Web.Http;
using MobileApi.Server.Core;
using MobileApi.Server.Models;
using NLog;

namespace MobileApi.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IUnitOfWork _unitOfWork;

        public AccountController()
        {
            _unitOfWork = Startup.GetUnitOfWorkInstance();
        }

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IIdentityResult result = await _unitOfWork.Identity.RegisterUserAsync(registerViewModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                Logger.Warn("User didn't able to register account");
                return errorResult;
            }

            Logger.Info("New user registered PhoneNumber = {0}.", registerViewModel.PhoneNumber);

            return Ok();
        }
        
        [HttpPost]
        [Route("VerifyPhoneNumber")]
        public async Task<IHttpActionResult> VerifyPhoneNumber(VerifyCodeViewModel verifyCodeViewModel)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _unitOfWork.Identity.VerifyPhoneNumberAsync(verifyCodeViewModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                Logger.Warn("User didn't able to verify PhoneNumber");
                return errorResult;
            }

            Logger.Info("User's PhoneNumber verified.");

            return Ok("Phone verified, just Reauthorize.");
        }

        #region < IDisposable >
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }

            base.Dispose(disposing);
        }
        #endregion

        #region < Helpers >
        private IHttpActionResult GetErrorResult(IIdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
        #endregion
    }
}
