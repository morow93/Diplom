using InoDrive.Api.Providers;
using InoDrive.Domain;
using InoDrive.Domain.Models;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Repositories.Abstract;
using Microsoft.AspNet.Identity;
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
    [RoutePrefix("api/trips")]
    public class TripsController : ApiController
    {
        private ITripsRepository _repo;

        public TripsController(ITripsRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Authorize]
        [Route("CreateTrip")]
        public async Task<IHttpActionResult> CreateTrip()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, new { status = Statuses.CommonFailure }));
            }
            var provider = GetMultipartProvider();
            var resultFile = await Request.Content.ReadAsMultipartAsync(provider);

            var trip = (InputCreateTripModel)GetFormData<InputCreateTripModel>(resultFile);

            var isFileAttached = resultFile.FileData.Count != 0;
            if (isFileAttached)
            {
                var fileInfo = (new FileInfo(resultFile.FileData.First().LocalFileName));
                trip.CarImage = fileInfo.Name;
                trip.CarImageExtension = fileInfo.Extension;
            }
            else
            {
                var carsFolder = ConfigurationManager.AppSettings["carsFolder"];
                string fullPathToFile = HttpContext.Current.Server.MapPath(carsFolder + trip.CarImage);

                if (File.Exists(fullPathToFile))
                {
                    var fileInfo = (new FileInfo(fullPathToFile));

                    var guid = Guid.NewGuid().ToString();
                    var extension = fileInfo.Extension;
                    var name = "BodyPart_" + guid + extension;

                    string fullPathToNewFile = HttpContext.Current.Server.MapPath(carsFolder + name);
                    File.Copy(fullPathToFile, fullPathToNewFile);

                    trip.CarImage = name;
                    trip.CarImageExtension = extension;
                }
            }

            try
            {
                _repo.CreateTrip(trip);
                return Ok(new { status = Statuses.CommonSuccess });
            }
            catch(Exception ex)
            {
                File.Delete(trip.CarImage);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("RemoveTrip")]
        public IHttpActionResult RemoveTrip(InputManageTripModel model)
        {
            try
            {
                _repo.RemoveTrip(model);
                return Ok();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("RecoverTrip")]
        public IHttpActionResult RecoverTrip(InputManageTripModel model)
        {
            try
            {
                _repo.RecoverTrip(model);
                return Ok();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Route("GetAllTrips")]
        public IHttpActionResult GetAllTrips(InputPageSortModel<Int32> model)
        {
            try
            {
                var result = _repo.GetAllTrips(model);
                return Ok(result);
            }
            catch(Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Route("GetDriverTrips")]
        public IHttpActionResult GetDriverTrips(InputPageSortModel<Int32> model)
        {
            try
            {
                var result = _repo.GetDriverTrips(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Route("GetPassengerTrips")]
        public IHttpActionResult GetPassengerTrips(InputPageSortModel<Int32> model)
        {
            try
            {
                var result = _repo.GetPassengerTrips(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetCar")]
        public async Task<IHttpActionResult> GetCar(ShortUserModel model)
        {
            try
            {
                var result = _repo.GetCar(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        #region Private functions

        private MultipartFormDataStreamProvider GetMultipartProvider()
        {
            var uploadFolder = ConfigurationManager.AppSettings["carsFolder"];
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
