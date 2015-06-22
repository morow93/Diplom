using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputFindTripsModel
    {
        public String OriginPlaceId { get; set; }
        public String DestinationPlaceId { get; set; }
        public DateTimeOffset LeavingDate { get; set; }
        public Int32 Places { get; set; }
        public Int32? PriceTop { get; set; }
        public Int32? PriceBottom { get; set; }
        public String Price { get; set; }

        public List<String> WayPoints { get; set; }

        public Boolean? IsAllowdedDeviation { get; set; }
        public Boolean? IsAllowdedChildren { get; set; }
        public Boolean? IsAllowdedPets { get; set; }
        public Boolean? IsAllowdedMusic { get; set; }
        public Boolean? IsAllowdedDrink { get; set; }
        public Boolean? IsAllowdedEat { get; set; }
        public Boolean? IsAllowdedSmoke { get; set; }

        public Int32 Page { get; set; }
        public Int32 PerPage { get; set; }
        public String SortOrder { get; set; }
        public String SortField { get; set; }
    }

}
