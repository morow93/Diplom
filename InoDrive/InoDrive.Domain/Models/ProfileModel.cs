using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Models
{
    public class ProfileModel
    {
        public String UserId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Int32? YearOfStage { get; set; }
        public Int32? YearOfBirth { get; set; }
        public String Phone { get; set; }
        public String About { get; set; }
        public String AvatarImage { get; set; }
        public String AvatarImageExtension { get; set; }
        public String OldAvatarImage { get; set; }
        public String OldAvatarImageExtension { get; set; }
        public Boolean Sex { get; set; }
    }
}
