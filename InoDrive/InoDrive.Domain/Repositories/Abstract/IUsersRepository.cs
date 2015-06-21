using InoDrive.Domain.Models;
using InoDrive.Domain.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Abstract
{
    public interface IUsersRepository
    {
        OutputUserSummaryModel GetUserSummary(ShortUserModel model);

        ProfileModel GetUserProfile(ShortUserModel model);

        void SetUserProfile(ProfileModel model);

        void SetUserCar(CarModel model);
    }
}
