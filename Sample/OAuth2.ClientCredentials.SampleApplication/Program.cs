using System;

namespace OAuth2.ClientCredentials.SampleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            // 1. Declare pre-requisites
            string tokenEndpoint = "https://security-token-service.com/oauth2/v1/token";
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";

            // 2. Create AuthRequest object
            AuthRequest authRequest = new AuthRequest(clientId, clientSecret);

            // 3. Create ClientCredential object
            ClientCredential defaultClientCredential = new ClientCredential(tokenEndpoint, authRequest);

            // 4. Obtain access token in auth response 
            AuthResponse authResponse = defaultClientCredential.GetToken().Result;

            Console.WriteLine("Token: " + authResponse.AccessToken);
            Console.ReadLine();
        }
    }
}