using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputChangeEmailModel: ShortUserModel
    {
        public String OldEmail { get; set; }
        public String NewEmail { get; set; }
    }
}
