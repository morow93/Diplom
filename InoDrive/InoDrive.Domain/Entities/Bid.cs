using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Entities
{
    public class Bid
    {
        public Int32 BidId { get; set; }

        public Boolean? IsAccepted { get; set; }
        public Boolean IsWatchedByOwnerUser { get; set; }
        public Boolean IsWatchedByAssignedUser { get; set; }
        public DateTime CreationDate { get; set; }

        public String UserId { get; set; }
        public Int32 TripId { get; set; }

        public virtual User User { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
