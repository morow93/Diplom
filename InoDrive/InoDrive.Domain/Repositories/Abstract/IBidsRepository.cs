using InoDrive.Domain.Models;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Abstract
{
    public interface IBidsRepository
    {
        #region Section of requests for updating counters, bids

        Int32 GetCountOfOwnBids(ShortUserModel model);
        Int32 GetCountOfAssignedBids(ShortUserModel model);
        //List<UnwatchedBidModel> GetUpdatedOwnBids(ShortUserModel model);
        //List<BidForMyTripModel> GetUpdatedAssignedBids(LoadBidsModel model);

        #endregion

        #region Section of main requests for select bids

        OutputList<OutputBidForMyTripModel> GetBidsForMyTrips(InputPageSortModel<Int32> model);
        OutputList<OutputMyBidModel> GetMyBids(InputPageSortModel<Int32> model);

        #endregion

        #region Add or update some bids entities

        void AddBid(InputManageBidModel model);
        void AcceptBid(InputManageBidModel model);
        void RejectBid(InputManageBidModel model);
        void WatchBid(InputManageBidModel model);

        #endregion
    }
}
