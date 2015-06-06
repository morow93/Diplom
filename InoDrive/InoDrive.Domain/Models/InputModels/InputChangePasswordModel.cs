using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputChangePasswordModel : ShortUserModel
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
    }
}
