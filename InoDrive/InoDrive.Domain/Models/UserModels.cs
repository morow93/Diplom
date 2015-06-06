using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models
{
    public class ShortUserModel
    {
        [Required]
        public String UserId { get; set; }
    }

    public class UserEmailModel
    {
        [Required]
        public String Email { get; set; }
    }

    public class UserModel
    {
        [Required]
        public String UserId { get; set; }

        [Required]
        public String FirstName { get; set; }

        [Required]
        public String LastName { get; set; }
    }
}
