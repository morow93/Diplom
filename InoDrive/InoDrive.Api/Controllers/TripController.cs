using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InoDrive.Api.Controllers
{
    [RoutePrefix("api/trips")]
    public class TripsController : ApiController
    {
        [HttpPost]
        [Authorize]
        [Route("test")]
        public IHttpActionResult Test()
        {
            return Ok("FUCK YEAH");
        }
    }
}
