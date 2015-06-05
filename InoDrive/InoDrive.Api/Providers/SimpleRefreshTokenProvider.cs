using InoDrive.Domain.Entities;
using InoDrive.Domain.Helpers;
using InoDrive.Domain.Repositories.Abstract;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InoDrive.Api.Providers
{
    /// <summary>
    /// Provides the work with refresh token
    /// </summary>
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public SimpleRefreshTokenProvider(Func<IAuthenticationRepository> authenticationRepositoryFactory)
        {
            this.authenticationRepositoryFactory = authenticationRepositoryFactory;
        }

        #region Private region

        private Func<IAuthenticationRepository> authenticationRepositoryFactory;

        private IAuthenticationRepository AuthenticationRepository
        {
            get
            {
                return this.authenticationRepositoryFactory.Invoke();
            }
        }

        #endregion

        /// <summary>
        /// This method is responsible to create or recreate refresh token
        /// </summary>
        /// <param name="context">Current authentication cotext</param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientId))
            {
                return;
            }

            var refreshTokenId = context.Ticket.Properties.Dictionary["refreshTokenId"];
            if (string.IsNullOrWhiteSpace(refreshTokenId))
            {
                refreshTokenId = Guid.NewGuid().ToString("n");
                context.Ticket.Properties.Dictionary["refreshTokenId"] = refreshTokenId;
            }

            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            var token = new RefreshToken()
            {
                Id = Helper.GetHash(refreshTokenId),
                ClientId = clientId,
                Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;

            token.ProtectedTicket = context.SerializeTicket();

            var result = await AuthenticationRepository.AddRefreshToken(token);

            if (result)
            {
                context.SetToken(refreshTokenId);
            }
        }

        /// <summary>
        /// This method implements the logic needed once we receive the refresh token
        /// so we can generate a new access token 
        /// </summary>
        /// <param name="context">Current authentication cotext</param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            string hashedTokenId = Helper.GetHash(context.Token);

            var refreshToken = AuthenticationRepository.FindRefreshToken(hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                var result = await AuthenticationRepository.RemoveRefreshToken(hashedTokenId);
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}