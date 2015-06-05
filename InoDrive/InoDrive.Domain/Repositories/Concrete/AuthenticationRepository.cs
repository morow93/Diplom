using InoDrive.Domain.Contexts;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Helpers;
using InoDrive.Domain.Models.InputModels;
using InoDrive.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Repositories.Concrete
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public AuthenticationRepository(AuthContext authContext)
        {
            _ctx = authContext;
        }

        private AuthContext _ctx;

        /// <summary>
        /// Find reshresh token by its id
        /// </summary>
        /// <param name="refreshTokenId">token id</param>
        /// <returns></returns>
        public RefreshToken FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = _ctx.RefreshTokens.SingleOrDefault(r => r.Id == refreshTokenId);

            return refreshToken;
        }

        /// <summary>
        /// Find client by its id
        /// </summary>
        /// <param name="clientId">client id</param>
        /// <returns></returns>
        public Client FindClient(string clientId)
        {
            var client = _ctx.Clients.Find(clientId);

            return client;
        }

        /// <summary>
        /// Add refresh token to client (remove if exist and create new)
        /// </summary>
        /// <param name="token">token subject and client id</param>
        /// <returns></returns>
        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = _ctx.RefreshTokens.SingleOrDefault(r => r.Id == token.Id);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Remove refresh token by its id
        /// </summary>
        /// <param name="refreshTokenId">token id</param>
        /// <returns></returns>
        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _ctx.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _ctx.RefreshTokens.Remove(refreshToken);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Remove refresh token by its entity
        /// </summary>
        /// <param name="refreshToken">refresh token</param>
        /// <returns></returns>
        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _ctx.RefreshTokens.Remove(refreshToken);
            return await _ctx.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Remove refresh token
        /// </summary>
        /// <param name="model">InputRemoveRefreshTokenModel variable</param>
        /// <returns></returns>
        public Boolean RemoveRefreshToken(InputRemoveRefreshTokenModel model)
        {
            var hashedId = Helper.GetHash(model.RefreshToken);
            var tokenToRemove = _ctx.RefreshTokens.FirstOrDefault(t => t.Subject == model.UserName && t.Id == hashedId);

            if (tokenToRemove == null) return false;

            var resutl = true;
            try
            {
                _ctx.RefreshTokens.Remove(tokenToRemove);
                _ctx.SaveChanges();
            }
            catch
            {
                resutl = false;
            }
            return resutl;
        }

        /// <summary>
        /// Remove all expired refresh tokens
        /// </summary>
        /// <returns></returns>
        public Boolean RemoveAllExpiredRefreshTokens()
        {
            var result = true;

            var removeDate = DateTime.UtcNow.AddDays(1);

            var tokensToRemove = _ctx.RefreshTokens.Where(rt => removeDate > rt.ExpiresUtc);

            if (tokensToRemove.Any())
            {
                try
                {
                    _ctx.RefreshTokens.RemoveRange(tokensToRemove);
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
