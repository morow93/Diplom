using InoDrive.Domain.Entities;
using InoDrive.Domain.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Abstract
{
    public interface IAuthenticationRepository
    {
        Client FindClient(string clientId);
        RefreshToken FindRefreshToken(string refreshTokenId);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
        Boolean RemoveRefreshToken(InputRemoveRefreshTokenModel model);
        Boolean RemoveAllExpiredRefreshTokens();
    }
}
