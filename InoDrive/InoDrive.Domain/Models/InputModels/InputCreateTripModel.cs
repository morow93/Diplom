using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputCreateTripModel
    {
        public String UserId { get; set; }
        public PlaceModel OriginPlace { get; set; }
        public PlaceModel DestinationPlace { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public Int32 PeopleCount { get; set; }
        public Int32? Pay { get; set; }
        public List<PlaceModel> SelectedPlaces { get; set; }
        public String Car { get; set; }
        public String CarClass { get; set; }
        public String CarImage { get; set; }
        public String CarImageExtension { get; set; }
        public Boolean IsAllowdedDeviates { get; set; }
        public Boolean IsAllowdedPets { get; set; }
        public Boolean IsAllowdedChildren { get; set; }
        public Boolean IsAllowdedMusic { get; set; }
        public Boolean IsAllowdedDrink { get; set; }
        public Boolean IsAllowdedEat { get; set; }
        public Boolean IsAllowdedSmoke { get; set; }
        public String About { get; set; }
    }
}
