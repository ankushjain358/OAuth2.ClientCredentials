using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace OAuth2.ClientCredentials
{
    /// <summary>
    /// Cache Manager class takes care of caching auth tokens
    /// Cache Manager is implemented as Singleton 
    /// Cache Manager is protected as Internal so that its consumers can not play with it
    /// </summary>
    internal class CacheManager
    {
        /// <summary>
        /// This field holds the instance of singleton class
        /// </summary>
        private static CacheManager instance = null;

        /// <summary>
        /// This object is used for locking
        /// </summary>
        private static readonly object padlock = new object();

        /// <summary>
        /// Private constructor
        /// </summary>
        private CacheManager()
        {
        }

        /// <summary>
        /// Returns the instance of CacheManager class
        /// </summary>
        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new CacheManager();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// This field stores the cached requests and responses
        /// </summary>
        private Dictionary<string, string> dictionary = new Dictionary<string, string>();

        /// <summary>
        /// This method returns tokens from cache
        /// </summary>
        /// <param name="authRequest">Auth reqeust</param>
        /// <returns>Returns auth response if found else returns null</returns>
        public AuthResponse GetTokenFromCache(AuthRequest authRequest)
        {
            string requestJson = JsonConvert.SerializeObject(authRequest);
            AuthResponse authResponse = null;

            if (dictionary.ContainsKey(requestJson))
            {
                string responseJson = dictionary[requestJson];

                authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseJson);

                if (IsExpired(authResponse))
                {
                    // Set authResponse to null
                    authResponse = null;

                    // Also, remove this entry from cached tokens
                    dictionary.Remove(requestJson);
                }
            }
            return authResponse;
        }

        /// <summary>
        /// This method is used to add token in cache
        /// </summary>
        /// <param name="authRequest">Auth request</param>
        /// <param name="authResponse">Auth reponse</param>
        public void AddTokenToCache(AuthRequest authRequest, AuthResponse authResponse)
        {
            string requestJson = JsonConvert.SerializeObject(authRequest);

            // Add only if the auth request/response is not already cached
            if (!dictionary.ContainsKey(requestJson))
            {
                dictionary.Add(requestJson, JsonConvert.SerializeObject(authResponse));
            }
        }

        /// <summary>
        /// This method is used to intentionally clear the cache
        /// </summary>
        public void CleanCache()
        {
            dictionary.Clear();
        }

        #region Private Methods

        /// <summary>
        /// This method is used to check if token is expired or not
        /// </summary>
        /// <param name="authResponse">Auth response</param>
        /// <returns>Returns true or false</returns>
        private bool IsExpired(AuthResponse authResponse)
        {
            bool isExpired = false;
            JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = securityTokenHandler.ReadToken(authResponse.AccessToken) as JwtSecurityToken;
            var tokenExpiryDate = jwtToken.ValidTo;

            // reduce expiry datetime by 5 minutes so that we can renew it 5 minutes earlier before it expires
            tokenExpiryDate = tokenExpiryDate.AddMinutes(-5);

            // If there is no valid `exp` claim then `ValidTo` returns DateTime.MinValue
            if (tokenExpiryDate == DateTime.MinValue)
                isExpired = true;

            // If the token is in the past then you can't use it
            if (tokenExpiryDate < DateTime.UtcNow)
                isExpired = true;

            return isExpired;
        }

        #endregion

    }
}
