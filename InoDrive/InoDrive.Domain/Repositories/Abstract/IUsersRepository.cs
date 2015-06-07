using InoDrive.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Abstract
{
    public interface IUsersRepository
    {
        ProfileModel GetUserProfile(ShortUserModel model);
        void SetUserProfile(ProfileModel model);
    }
}
