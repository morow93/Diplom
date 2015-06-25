using InoDrive.Domain;
using InoDrive.Domain.Models;
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

        #region Section of requests for updating counters, bids

        [HttpPost]
        [Authorize]
        [Route("GetCountOfOwnBids")]
        public IHttpActionResult GetCountOfOwnBids(ShortUserModel model)
        {
            try
            {
                var result = _repo.GetCountOfOwnBids(model);
                return Ok(new { Count = result });

            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetCountOfAssignedBids")]
        public IHttpActionResult GetCountOfAssignedBids(ShortUserModel model)
        {
            try
            {
                var result = _repo.GetCountOfAssignedBids(model);
                return Ok(new { Count = result });
            }
            catch (Exception error)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        //[HttpPost]
        //[Authorize]
        //[Route("GetUpdatedOwnBids")]
        //public HttpResponseMessage GetUpdatedOwnBids(ShortUserModel model)
        //{
        //    var errorTitle = "Произошла ошибка при получении ваших заявок с обновленным статусом!";
        //    try
        //    {
        //        var result = _repo.GetUpdatedOwnBids(model);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (AlertException error)
        //    {
        //        var jsonError = new
        //        {
        //            Title = errorTitle,
        //            Content = error.Message,
        //            NeedAlert = true
        //        };
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, jsonError);
        //    }
        //    catch (RedirectException error)
        //    {
        //        var jsonError = new
        //        {
        //            Title = errorTitle,
        //            Content = error.Message,
        //            NeedRedirect = true
        //        };
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, jsonError);
        //    }
        //    catch (Exception error)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, error);
        //    }
        //}

        //[HttpPost]
        //[Authorize]
        //[Route("GetUpdatedAssignedBids")]
        //public HttpResponseMessage GetUpdatedAssignedBids(LoadBidsModel model)
        //{
        //    var errorTitle = "Произошла ошибка при получении новых заявок на ваши поездки!";
        //    try
        //    {
        //        var result = _repo.GetUpdatedAssignedBids(model);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    }
        //    catch (AlertException error)
        //    {
        //        var jsonError = new
        //        {
        //            Title = errorTitle,
        //            Content = error.Message,
        //            NeedAlert = true
        //        };
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, jsonError);
        //    }
        //    catch (RedirectException error)
        //    {
        //        var jsonError = new
        //        {
        //            Title = errorTitle,
        //            Content = error.Message,
        //            NeedRedirect = true
        //        };
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, jsonError);
        //    }
        //    catch (Exception error)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, error);
        //    }
        //}

        #endregion

        #region Section of main requests for select bids

        [HttpPost]
        [Authorize]
        [Route("GetMyBids")]
        public IHttpActionResult GetMyBids(InputPageSortModel<Int32> model)
        {
            try
            {
                var result = _repo.GetMyBids(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetBidsForMyTrips")]
        public IHttpActionResult GetBidsForMyTrips(InputPageSortModel<Int32> model)
        {
            try
            {
                var result = _repo.GetBidsForMyTrips(model);
                return Ok(result);
            }
            catch (Exception error)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        #endregion

        #region Add or update some bids entities

        [HttpPost]
        [Authorize]
        [Route("AddBid")]
        public IHttpActionResult AddBid(InputManageBidModel model)
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

        [HttpPost]
        [Authorize]
        [Route("AcceptBid")]
        public IHttpActionResult AcceptBid(InputManageBidModel bid)
        {
            try
            {
                _repo.AcceptBid(bid);
                return Ok();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("RejectBid")]
        public IHttpActionResult RejectBid(InputManageBidModel bid)
        {
            try
            {
                _repo.RejectBid(bid);
                return Ok();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("WatchBid")]
        public IHttpActionResult WatchBid(InputManageBidModel bid)
        {
            try
            {
                _repo.WatchBid(bid);
                return Ok();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, new { status = Statuses.CommonFailure }));
            }
        }

        #endregion
    }
}
