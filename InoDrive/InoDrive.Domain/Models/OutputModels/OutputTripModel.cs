using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class UserIndicator
    {
        public String UserId { get; set; }
        public String AvatarImage { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Boolean WasTriped { get; set; }
    }

    public class OutputTripModel
    {
        public Int32 TripId { get; set; }
        public String UserId { get; set; }
        public String Initials { get; set; }
        public PlaceModel OriginPlace { get; set; }
        public PlaceModel DestinationPlace { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public String Car { get; set; }
        public String CarImage { get; set; }
        public Int32 FreePlaces { get; set; }
        public Int32 TotalPlaces { get; set; }
        public List<PlaceModel> WayPoints { get; set; }
        public List<UserIndicator> UserIndicators { get; set; }
        public Decimal? Pay { get; set; }
        public Boolean IsBidded { get; set; }
        public Boolean IsEnded { get; set; }
        public List<OutputCommentModel> Comments { get; set; }
        public Boolean AllowCommented { get; set; }
    }
}
