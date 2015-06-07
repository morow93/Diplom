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
        public Int32 WayPointIndex { get; set; }

        public Int32 TripId { get; set; }
        public String PlaceId { get; set; }

        public virtual Place Place { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
