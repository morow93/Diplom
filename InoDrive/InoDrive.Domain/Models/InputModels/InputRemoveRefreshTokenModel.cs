using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputRemoveRefreshTokenModel
    {
        public String UserName { get; set; }
        public String OriginToken { get; set; }
        public String RefreshToken { get; set; }
    }
}
