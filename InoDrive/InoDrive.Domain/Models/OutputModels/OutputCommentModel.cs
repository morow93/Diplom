using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.OutputModels
{
    public class OutputCommentModel
    {
        public Int32 CommentId { get; set; }
        public String Title { get; set; }
        public Int32 Vote { get; set; }
        public String Initials { get; set; }
        public String UserId { get; set; }
    }
}
