using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OAuth2.ClientCredentials
{
    /// <summary>
    /// This class is used to generate token using Client Credentials grant type
    /// </summary>
    public class ClientCredential
    {
        /// <summary>
        /// Gets or sets token endpoint
        /// </summary>
        private string TokenEndpoint { get; set; }

        /// <summary>
        /// Gets or sets auhentication request object
        /// </summary>
        private AuthRequest AuthRequest { get; set; }

        /// <summary>
        /// Constructor for ClientCredential class
        /// </summary>
        /// <param name="tokenEndpoint">Token endpoint</param>
        /// <param name="authRequest">Authentication request</param>
        public ClientCredential(string tokenEndpoint, AuthRequest authRequest)
        {
            // validate auth request and throw exception as soon as request is found invalid
            ValidateAuthRequest(tokenEndpoint, authRequest);

            // trim parameters to remove white spaces
            TrimRequestParameters(authRequest);

            // assign member variables
            TokenEndpoint = tokenEndpoint.Trim();
            AuthRequest = authRequest;
        }

        /// <summary>
        /// This method is used to obtain access token from auth server
        /// </summary>
        /// <returns>Returns auth response</returns>
        public async Task<AuthResponse> GetToken()
        {
            try
            {
                // Get token from cache
                AuthResponse cachedAuthResponse = CacheManager.Instance.GetTokenFromCache(AuthRequest);

                if (cachedAuthResponse != null)
                {
                    // Return the cached response
                    return cachedAuthResponse;
                }
                else
                {
                    const string requestParam = "scope={0}&client_id={1}&grant_type=client_credentials&client_secret={2}";

                    var payload = string.Format(requestParam,
                                    WebUtility.UrlEncode(AuthRequest.Scope),
                                    WebUtility.UrlEncode(AuthRequest.ClientId),
                                    WebUtility.UrlEncode(AuthRequest.ClientSecret));

                    var content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(TokenEndpoint, content);

                        // this will throw an exception when status code is not 200
                        response.EnsureSuccessStatusCode();

                        // code will reach here only if status code is 200
                        var responseAsString = await response.Content.ReadAsStringAsync();
                        var responseAsObject = JsonConvert.DeserializeObject<AuthResponse>(responseAsString);

                        // Add token in cache
                        CacheManager.Instance.AddTokenToCache(AuthRequest, responseAsObject);

                        // Finally return the response
                        return responseAsObject;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to intentionally clear the cache
        /// </summary>
        public static void ClearCache()
        {
            CacheManager.Instance.CleanCache();
        }

        #region Private Methods

        /// <summary>
        /// This method is used to validate auth request
        /// </summary>
        /// <param name="tokenEndpoint">Token endpoint</param>
        /// <param name="authRequest">Auth request</param>
        private void ValidateAuthRequest(string tokenEndpoint, AuthRequest authRequest)
        {
            if (string.IsNullOrWhiteSpace(tokenEndpoint))
                throw new ArgumentException("Token endpoint can not be null or empty");

            if (authRequest == null)
                throw new ArgumentNullException("Auth request can not be null");

            if (string.IsNullOrWhiteSpace(authRequest.ClientId))
                throw new ArgumentException("Client can not be null or empty");

            if (string.IsNullOrWhiteSpace(authRequest.ClientSecret))
                throw new ArgumentException("Client Secret can not be null or empty");
        }

        /// <summary>
        /// This methos is used to trim the values to remove the white-spaces
        /// </summary>
        /// <param name="authRequest">Auth request object</param>
        private void TrimRequestParameters(AuthRequest authRequest)
        {
            authRequest.ClientId = authRequest.ClientId.Trim();
            authRequest.ClientSecret = authRequest.ClientSecret.Trim();
            authRequest.Scope = (authRequest.Scope ?? string.Empty).Trim();
        }

        #endregion

    }
}
