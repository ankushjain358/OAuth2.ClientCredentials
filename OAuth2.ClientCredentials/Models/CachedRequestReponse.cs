namespace OAuth2.ClientCredentials
{
    /// <summary>
    /// This class is used by cache manager
    /// </summary>
    public class CachedRequestReponse
    {
        /// <summary>
        /// Constructor for CachedRequestReponse
        /// </summary>
        /// <param name="request">Auth request</param>
        /// <param name="response">Auth response</param>
        public CachedRequestReponse(AuthRequest request, AuthResponse response)
        {
            Request = request;
            Response = response;
        }

        /// <summary>
        /// Gets or sets authentication request
        /// </summary>
        public AuthRequest Request { get; set; }

        /// <summary>
        ///  Gets or sets authentication response
        /// </summary>
        public AuthResponse Response { get; set; }
    }
}
