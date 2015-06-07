using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models
{
    public class PlaceModel
    {
        public String GooglePlaceId { get; set; }
        public String Name { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}
