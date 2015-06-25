using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputManageBidModel
    {
        public String UserId { get; set; }
        public Int32 TripId { get; set; }
        public Int32 BidId { get; set; }
        public String UserOwnerId { get; set; }
        public String UserClaimedId { get; set; }
    }
}
