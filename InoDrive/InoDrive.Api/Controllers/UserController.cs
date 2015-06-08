using InoDrive.Api.Providers;
using InoDrive.Domain;
using InoDrive.Domain.Models;
using InoDrive.Domain.Repositories.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace InoDrive.Api.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private IUsersRepository _repo;

        public UserController(IUsersRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Authorize]
        [Route("getUserProfile")]
        public IHttpActionResult GetUserProfile(ShortUserModel model)
        {
            var result = _repo.GetUserProfile(model);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("setUserProfile")]
        public async Task<IHttpActionResult> SetUserProfile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }
            var provider = GetMultipartProvider();
            var resultFile = await Request.Content.ReadAsMultipartAsync(provider);

            var profile = (ProfileModel)GetFormData<ProfileModel>(resultFile);

            var isFileAttached = resultFile.FileData.Count != 0;
            if (isFileAttached)
            {
                profile.AvatarImage = (new FileInfo(resultFile.FileData.First().LocalFileName)).Name;
            }

            if (profile.AvatarImage != profile.OldAvatarImage)
            {
                string fullPathToFile =
                    HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["avatarsFolder"] + profile.OldAvatarImage);
                if (File.Exists(fullPathToFile))
                {
                    File.Delete(fullPathToFile);
                }
            }

            try
            {
                _repo.SetUserProfile(profile);
            }
            catch
            {
                File.Delete(profile.AvatarImage);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
            return Ok(profile);
        }

        #region Private functions

        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = ConfigurationManager.AppSettings["avatarsFolder"];
            var root = HttpContext.Current.Server.MapPath(uploadFolder);
            Directory.CreateDirectory(root);

            return new CustomMultipartFormDataStreamProvider(root);
        }

        private object GetFormData<T>(MultipartFormDataStreamProvider result)
        {
            if (result.FormData.HasKeys())
            {
                var unescapedFormData = Uri.UnescapeDataString(result.FormData.GetValues(0).FirstOrDefault() ?? String.Empty);
                if (!String.IsNullOrEmpty(unescapedFormData))
                    return JsonConvert.DeserializeObject<T>(unescapedFormData);
            }
            return null;
        }

        #endregion

    }
}
