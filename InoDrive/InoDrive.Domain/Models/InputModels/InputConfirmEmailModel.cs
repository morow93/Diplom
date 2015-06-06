using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models.InputModels
{
    public class InputConfirmEmailModel
    {
        [Required]
        public String UserId { get; set; }
        [Required]
        public String Code { get; set; }
    }
}
