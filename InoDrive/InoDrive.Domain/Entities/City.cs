using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    public class Place
    {
        public Place() { }
        public Int32 PlaceId { get; set; }

        public String GooglePlaceId { get; set; }
        public String Name { get; set; }
        public String AdditionalInfo { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}
