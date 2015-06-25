using InoDrive.Domain.Models.InputModels;
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

        //Int32 GetCountOfOwnBids(ShortUserModel model);
        //Int32 GetCountOfAssignedBids(ShortUserModel model);
        //List<UnwatchedBidModel> GetUpdatedOwnBids(ShortUserModel model);
        //List<BidForMyTripModel> GetUpdatedAssignedBids(LoadBidsModel model);

        #endregion

        #region Section of main requests for select bids

        //ResultBidsForMyTripsModel GetBidsForMyTrips(BidsForMyTripsPagedOrderModel model);
        //ResultMyBidsModel GetMyBids(MyBidsPagedOrderModel model);

        #endregion

        #region Add or update some bids entities

        void AddBid(InputManageTripModel model);
        //void AcceptBid(InputManageTripModel model);
        //void RejectBid(InputManageTripModel model);
        //void WatchBid(InputManageTripModel model);

        #endregion
    }
}
