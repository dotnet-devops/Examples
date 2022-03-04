using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiDemo.Library.Graph
{
    public class MsalWrappedTokenProvider : IAuthenticationProvider
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="accessTokenProvider">WebAssembly access token provider instance</param>
        public MsalWrappedTokenProvider(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }

        private const string MicrosoftGraphScope = "Sites.Read.All";
       

        private string[] GetRelevantScopes(Uri resourceUri)
        {
                return new[] { $"{resourceUri}/{MicrosoftGraphScope}" };
        }

        /// <summary>
        /// Authenticate the web request
        /// </summary>
        /// <param name="resource">Resource to get an access token for</param>
        /// <param name="request">Request to add the access token on</param>
        /// <returns></returns>
        public async Task AuthenticateRequestAsync(Uri resource, HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer",
                await GetAccessTokenAsync(resource).ConfigureAwait(false));
        }

        /// <summary>
        /// Gets an access token for the requested resource and scopes
        /// </summary>
        /// <param name="resource">Resource to get access token for</param>
        /// <param name="scopes">Scopes to use when getting the access token</param>
        /// <returns>Obtained access token</returns>
        public async Task<string> GetAccessTokenAsync(Uri resource, string[] scopes)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (scopes == null)
            {
                throw new ArgumentNullException(nameof(scopes));
            }

            var tokenResult = await _accessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions()
            {
                // The scopes must specify the needed permissions for the app to work
                Scopes = scopes,
            }).ConfigureAwait(false);

            if (!tokenResult.TryGetToken(out AccessToken accessToken))
            {
                throw new Exception("An error occured while trying to acquire the access token...");
            }

            return accessToken.Value;
        }

        /// <summary>
        /// Gets an access token for the requested resource
        /// </summary>
        /// <param name="resource">Resource to get access token for</param>
        /// <returns>Obtained access token</returns>
        public async Task<string> GetAccessTokenAsync(Uri resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            return await GetAccessTokenAsync(resource, GetRelevantScopes(resource));
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer",
                await GetAccessTokenAsync(new("https://graph.microsoft.com/" + MicrosoftGraphScope)).ConfigureAwait(false));
        }
    }
}
