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
    public interface ITripsRepository
    {
        #region Select

        OutputList<OutputMyTripModel> GetAllTrips(InputPageSortModel<Int32> model);
        OutputList<OutputMyTripModel> GetDriverTrips(InputPageSortModel<Int32> model);
        OutputList<OutputMyTripModel> GetPassengerTrips(InputPageSortModel<Int32> model);

        InputEditTripModel GetTripForEdit(InputManageTripModel model);
        CarModel GetCar(ShortUserModel model);

        //DetailsTripModel GetDetailsTrip(ManageTripModel model);
        //EditTripModel GetFullTripInfo(ManageTripModel model);
        //ResultFindTripsModel FindTrips(FindTripsPagedOrderModel model);
        //ResultFindTripsModel ExtendFindTrips(ExtendTripsPagedOrderModel model);
        //ResultMyTripsModel GetMyTrips(MyTripsPagedOrderModel model);

        #endregion

        #region Modifications

        void CreateTrip(InputCreateTripModel model);
        void RemoveTrip(InputManageTripModel model);
        void RecoverTrip(InputManageTripModel model);
        void EditTrip(InputCreateTripModel model);

        //void VoteForTrip(VoteTripModel model);

        #endregion
    }
}
