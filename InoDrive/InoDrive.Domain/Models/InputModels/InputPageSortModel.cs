using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputPageSortModel<T>
    {
        public Int32 Page { get; set; }
        public Int32 PerPage { get; set; }
        public T FromId { get; set; }
        public String SortField { get; set; }
        public String SortOrder { get; set; }

        public Int32 CountExcluded { get; set; }
        public String UserId { get; set; }
        public Boolean ShowEnded { get; set; }
        public Boolean IsOwner { get; set; }
    }
}
