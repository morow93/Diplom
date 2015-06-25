using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InpuCommentModel
    {
        public String UserId { get; set; }
        public Int32 TripId { get; set; }
        public String Title { get; set; }
        public Int32 Vote { get; set; }
    }
}
