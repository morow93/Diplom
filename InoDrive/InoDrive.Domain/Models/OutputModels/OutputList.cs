using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class OutputList<T>
    {
        public List<T> Results { get; set; }
        public Int32 TotalCount { get; set; }
    }
}
