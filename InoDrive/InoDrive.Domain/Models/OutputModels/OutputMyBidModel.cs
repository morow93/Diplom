using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class OutputMyBidModel
    {
        public Int32 BidId { get; set; }
        public Int32 TripId { get; set; }
        public PlaceModel OriginPlace { get; set; }
        public PlaceModel DestinationPlace { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Int32 FreePlaces { get; set; }
        public Int32 TotalPlaces { get; set; }
        public UserModel UserOwner { get; set; }
        public Boolean? IsAccepted { get; set; }//null -> in the consideration, true -> accepted, false -> rejected
        public Boolean IsWatched { get; set; }
        public Boolean WasTripDeleted { get; set; }
    }
}
