using Newtonsoft.Json;

namespace OAuth2.ClientCredentials
{
    /// <summary>
    /// This class holds authentication response
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Gets or sets token type
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets expire time duration
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets access token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
