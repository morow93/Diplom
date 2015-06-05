using InoDrive.Api.Identity;
using InoDrive.Domain;
using InoDrive.Domain.Entities;
using InoDrive.Domain.Helpers;
using InoDrive.Domain.Repositories.Abstract;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace InoDrive.Api.Providers
{
    /// <summary>
    /// Provides the work with access token
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public SimpleAuthorizationServerProvider(Func<IAuthenticationRepository> authenticationRepositoryFactory, Func<ApplicationUserManager> applicationUserManagerFactory)
        {
            this.authenticationRepositoryFactory = authenticationRepositoryFactory;
            this.applicationUserManagerFactory = applicationUserManagerFactory;
        }

        #region Private region

        private Func<IAuthenticationRepository> authenticationRepositoryFactory;

        private Func<ApplicationUserManager> applicationUserManagerFactory;

        private IAuthenticationRepository authenticationRepository
        {
            get
            {
                return this.authenticationRepositoryFactory.Invoke();
            }
        }

        private ApplicationUserManager applicationUserManager
        {
            get
            {
                return this.applicationUserManagerFactory.Invoke();
            }
        }

        #endregion

        /// <summary>
        /// This method is responsible for validating the "Client"
        /// </summary>
        /// <param name="context">Current authentication context</param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";
            context.OwinContext.Response.Headers.Set("Access-Control-Allow-Origin", allowedOrigin);

            var clientId = string.Empty;
            var clientSecret = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.Validated();//context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            client = authenticationRepository.FindClient(context.ClientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(clientSecret))
                {
                    context.SetError("invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (client.Secret != Helper.GetHash(clientSecret))
                    {
                        context.SetError("invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!client.Active)
            {
                context.SetError("invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// This method is responsible to validate the username and password sent 
        /// to the authorization server’s token endpoint
        /// </summary>
        /// <param name="context">Current authentication context</param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            ApplicationUser user = await applicationUserManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            //if (!user.EmailConfirmed)
            //{
            //    context.SetError("not_confirmed_email", "Need to confirm your email adress!", user.Id);
            //    return;
            //}

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var props = new AuthenticationProperties(new Dictionary<string, string>
            {
                { 
                    "as:client_id", context.ClientId ?? string.Empty
                }
                ,{ 
                    "userName", context.UserName
                }
                ,{
                    "userId", user.Id
                }
                ,{
                    "refreshTokenId", ""
                }
                ,{
                    "initials", user.FirstName + " " + user.LastName
                }
            });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
        }

        /// <summary>
        /// This method implements the logic which allows us to issue new claims or updating existing claims 
        /// and contain them into the new access token generated before sending it to the user
        /// </summary>
        /// <param name="context">Current authentication context</param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            var newClaim = newIdentity.Claims.FirstOrDefault(c => c.Type == "newClaim");
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            //need do this cause user may change first or last name
            //var userId = context.Ticket.Properties.Dictionary["userId"];
            //ApplicationUser user = await applicationUserManager.FindByIdAsync(userId);
            //context.Ticket.Properties.Dictionary["initials"] = user.FirstName + " " + user.LastName;

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}