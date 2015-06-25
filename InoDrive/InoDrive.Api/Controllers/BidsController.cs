using InoDrive.Domain;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InoDrive.Api.Controllers
{
    [RoutePrefix("api/bids")]
    public class BidsController : ApiController
    {
        private IBidsRepository _repo;

        public BidsController(IBidsRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        [Authorize]
        [Route("AddBid")]
        public IHttpActionResult AddBid(InputManageTripModel model)
        {
            try
            {
                _repo.AddBid(model);
                return Ok();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }
    }
}
