using Newtonsoft.Json;

namespace OAuth2.ClientCredentials
{
    /// <summary>
    /// This class is used for authentication request
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Constructor for AuthRequest
        /// </summary>
        /// <param name="clientId">Client Id</param>
        /// <param name="clientSecret">Client Secret</param>
        /// <param name="scope">Scopes</param>
        public AuthRequest(string clientId, string clientSecret, string scope = "")
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Scope = scope;
            GrantType = "client_credentials";
        }

        /// <summary>
        /// Gets or sets client id
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets client secret
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets grant type
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; }

        /// <summary>
        /// Gets or sets scopes
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
