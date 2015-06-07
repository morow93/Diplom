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
        public Boolean IsWatchedBySender { get; set; }
        public Boolean IsWatchedByReceiver { get; set; }
        public DateTimeOffset CreationDate { get; set; }

        public String UserId { get; set; }
        public Int32 TripId { get; set; }

        public virtual User User { get; set; }
        public virtual Trip Trip { get; set; }
    }
}
