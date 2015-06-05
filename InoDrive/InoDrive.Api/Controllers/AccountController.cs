using InoDrive.Api.Identity;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Repositories.Abstract;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace InoDrive.Api.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IAuthenticationRepository _authenticationRepository;

        public AccountController(ApplicationUserManager userManager, IAuthenticationRepository authenticationRepository)
        {
            _userManager = userManager;
            _authenticationRepository = authenticationRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(InputSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                //,Email = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok("User was successfully signed up.");
        }

        [HttpPost]
        [Authorize]
        [Route("removeRefreshToken")]
        public IHttpActionResult RemoveRefreshToken(InputRemoveRefreshTokenModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authenticationRepository.RemoveRefreshToken(model);

            return Ok("Refresh token was successfuly removed.");
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
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
    }
}
