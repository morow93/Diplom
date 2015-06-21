using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class OutputFindTripModel
    {
        public Int32 TripId { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsEnded { get; set; }
        public Boolean IsStarted { get; set; }
        public Boolean AllowManage { get; set; }
        public Double? Pay { get; set; }
        public String OriginaPlaceId { get; set; }
        public String OriginPlaceName { get; set; }
        public String DestinationPlaceId { get; set; }
        public String DestinationPlaceName { get; set; }
        public String UserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Int32 TotalPlaces { get; set; }
        public Int32 FreePlaces { get; set; }
        public List<PlaceModel> Places { get; set; }
        public Double Rating { get; set; }
    }
}
