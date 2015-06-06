using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    public class WayPoint
    {
        public Int32 WayPointId { get; set; }
        public Int32 Index { get; set; }

        public Int32 TripId { get; set; }
        public Int32 CityId { get; set; }

        public virtual Place City { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
