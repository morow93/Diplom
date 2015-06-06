using InoDrive.Api.Identity;
using InoDrive.Domain;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Helpers;
using InoDrive.Domain.Models;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Repositories.Abstract;
using Microsoft.AspNet.Identity;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
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

        #region Register

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
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName  
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            else
            {
                var code = _userManager.GenerateEmailConfirmationToken(user.Id);

                var emailModel = new InputEmailTemplateModel { Initials = user.FirstName + " " + user.LastName, UserId = user.Id, Code = code };
                var emailHtmlBody = GenerateEmailTemplate(emailModel, "ConfirmEmailTemplateMessage.cshtml");

                await _userManager.SendEmailAsync(user.Id, AppConstants.LETTER_CONFIRM_EMAIL_TITLE, emailHtmlBody);

                return Ok(new { status = Statuses.CommonSuccess });
            }
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public IHttpActionResult ConfirmEmail(InputConfirmEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.FindById(model.UserId);
            if (user == null)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.USER_NOT_FOUND });
            }

            model.Code = model.Code.Replace(" ", "+");//fix wrong encoding

            var result = _userManager.ConfirmEmail(model.UserId, model.Code);

            if (!result.Succeeded)
            {
                var strError = String.Join(" ", result.Errors);
                if (strError.Contains("Invalid token"))
                {
                    return Ok(new { status = Statuses.CommonFailure, message = AppConstants.INVALID_CONFIRM_EMAIL_CODE });
                }
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.EMAIL_WASNT_CONFIRMED });
            }

            _userManager.UpdateSecurityStamp(model.UserId);//for invalidate current token
            return Ok(new { status = Statuses.CommonSuccess, message = AppConstants.EMAIL_WAS_CONFIRMED });
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

        [HttpPost]
        [Route("SendConfirmEmailCode")]
        public async Task<IHttpActionResult> SendConfirmEmailCode(ShortUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _userManager.FindById(model.UserId);
                if (user != null)
                {
                    _userManager.UpdateSecurityStamp(model.UserId);//for invalidate token(previous)
                    var code = _userManager.GenerateEmailConfirmationToken(user.Id);

                    //create html body for email template
                    var emailModel = new InputEmailTemplateModel { Initials = user.FirstName + " " + user.LastName, UserId = user.Id, Code = code };
                    var emailHtmlBody = GenerateEmailTemplate(emailModel, "ConfirmEmailTemplateMessage.cshtml");
                    //send
                    await _userManager.SendEmailAsync(user.Id, AppConstants.LETTER_CONFIRM_EMAIL_TITLE, emailHtmlBody);
                    return Ok(new { status = Statuses.CommonSuccess, message = AppConstants.CONFRIM_LETTER_WAS_SENDED });
                }
                else
                {
                    return Ok(new { status = Statuses.CommonFailure, message = AppConstants.USER_NOT_FOUND });
                }
            }
            catch
            {
                return BadRequest();
            }
        }

        #endregion

        #region Reset password

        [HttpPost]
        [Route("SendResetPasswordCode")]
        public async Task<IHttpActionResult> SendResetPasswordCode(UserEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.FindByEmail(model.Email);
            if (user == null)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.USER_NOT_FOUND });
            }
            if (!user.EmailConfirmed)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.EMAIL_WASNT_CONFIRMED });
            }

            try
            {
                _userManager.UpdateSecurityStamp(user.Id);//for invalidate token(previous)
                var code = _userManager.GeneratePasswordResetToken(user.Id);

                //create html body for email template
                var emailModel = new InputEmailTemplateModel { Initials = user.FirstName + " " + user.LastName, UserId = user.Id, Code = code };
                var emailHtmlBody = GenerateEmailTemplate(emailModel, "ResetPasswordTemplateMessage.cshtml");
                //send         
                await _userManager.SendEmailAsync(user.Id, AppConstants.LETTER_RESET_PASSWORD_TITLE, emailHtmlBody);

                return Ok(new { status = Statuses.CommonSuccess, message = AppConstants.RESET_PASSWORD_WAS_SENDED });
            }
            catch
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.RESET_PASSWORD_WASNT_SENDED });
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public IHttpActionResult ResetPassword(InputResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.FindById(model.UserId);
            if (user == null)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.USER_NOT_FOUND });
            }
            if (!user.EmailConfirmed)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.EMAIL_WASNT_CONFIRMED });
            }

            model.Code = model.Code.Replace(" ", "+") + "==";//fix wrong encoding 

            var result = _userManager.ResetPassword(model.UserId, model.Code, model.NewPassword);

            if (!result.Succeeded)
            {
                var strError = String.Join(" ", result.Errors);
                if (strError.Contains("Invalid token"))
                {
                    return Ok(new { status = Statuses.CommonFailure, message = AppConstants.INVALID_RESET_PASSWORD_CODE });
                }
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.PASSWORD_WASNT_RESETED });
            }

            _userManager.UpdateSecurityStamp(model.UserId);//for invalidate token(current)
            return Ok(new { status = Statuses.CommonSuccess, message = AppConstants.PASSWORD_WAS_RESETED });
        }

        #endregion

        #region Change password & email

        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(InputChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.FindById(model.UserId);
            if (user == null)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.USER_NOT_FOUND });
            }
            if (!_userManager.CheckPassword(user, model.OldPassword))
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.WRONG_OLD_PASSWORD_VALUE });
            }

            try
            {
                _userManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);
                return Ok(new { status = Statuses.CommonSuccess, message = AppConstants.PASSWORD_WAS_RESETED });
            }
            catch
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.PASSWORD_WASNT_RESETED });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("ChangeEmail")]
        public IHttpActionResult ChangeEmail(InputChangeEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.FindById(model.UserId);
            if (user == null)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.USER_NOT_FOUND });
            }
            if (user.UserName != model.OldEmail)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.WRONG_OLD_EMAIL_VALUE });
            }
            if (_userManager.FindByEmail(model.NewEmail) != null)
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.EMAIL_ALREADY_EXIST });
            }

            try
            {
                user.UserName = model.NewEmail;
                user.Email = model.NewEmail;
                _userManager.Update(user);

                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.EMAIL_WAS_RESETED });

            }
            catch
            {
                return Ok(new { status = Statuses.CommonFailure, message = AppConstants.EMAIL_WASNT_RESETED });
            }
        }

        #endregion

        #region Private functions

        private String GenerateEmailTemplate(InputEmailTemplateModel emailModel, String viewName)
        {
            //build full path to template
            var emailTemplatesFolder = WebConfigurationManager.AppSettings["emailTemplatesFolder"];
            var fullPathToTemplate = HttpContext.Current.Server.MapPath(emailTemplatesFolder + viewName);
            //generate the email body from the template file.
            var templateService = new TemplateService();
            var emailHtmlBody = templateService.Parse(File.ReadAllText(fullPathToTemplate), emailModel, null, null);
            //return html template like string
            return emailHtmlBody;
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
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        #endregion
    }
}
