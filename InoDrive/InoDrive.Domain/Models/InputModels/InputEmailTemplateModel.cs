using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputEmailTemplateModel
    {
        public String Initials { get; set; }
        public String UserId { get; set; }
        public String Code { get; set; }
    }
}
