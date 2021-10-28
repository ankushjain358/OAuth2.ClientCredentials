using Microsoft.AspNetCore.Mvc;

namespace OAuth2.ClientCredentials.SampleWebApplication.Controllers
{
    public class ClientCredentialsController : Controller
    {
        public IActionResult Token()
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

            // 5. Put auth-response in viewbag
            ViewBag.AuthResponse = authResponse;

            return View();
        }
    }
}