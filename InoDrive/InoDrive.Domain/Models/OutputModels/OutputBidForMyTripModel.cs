using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class OutputBidForMyTripModel
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Int32 BidId { get; set; }
        public UserModel UserClaimed { get; set; }
        public Int32 TripId { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public Decimal? Pay { get; set; }
        public PlaceModel OriginPlace { get; set; }
        public PlaceModel DestinationPlace { get; set; }
        public Int32 TotalPlaces { get; set; }
        public Int32 FreePlaces { get; set; }
    }
}
