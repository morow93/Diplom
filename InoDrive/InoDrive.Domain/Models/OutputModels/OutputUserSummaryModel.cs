using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class OutputUserSummaryModel
    {
        public Boolean? Sex { get; set; }
        public Int32? Age { get; set; }
        public Int32? Stage { get; set; }
        public Int32 AllTripsCount { get; set; }
        public Int32 DriverTripsCount { get; set; }
        public Int32 PassengerTripsCount { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public Double Rating { get; set; }
        public String Initials { get; set; }
        public String UserId { get; set; }
        public String About { get; set; }
    }
}
