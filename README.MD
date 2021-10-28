# Identity.OAuth2.ClientCredentials Access Token Utility

**Identity.OAuth2.ClientCredentials** is a utility to retrieve access token from OAuth2.0 Server or Security Token Service (STS) using Client Credentials grant type.

The implementation is done using .NET Standard, so NuGet package supports both .NET Framework and .NET Core applications.

## Features
- Easy setup
- Access Token caching
- Auto renewal upon expiry

## Getting Started

1. Install the standard Nuget package into your application.
```
Install-Package Identity.OAuth2.ClientCredentials -Version 1.0.0
```

2. Import proper namespace

```
using OAuth2.ClientCredentials;
```

3. Use below code to retrieve Access Token
```
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

// 5. Use the token
Console.WriteLine("Token: " + authResponse.AccessToken);
```

Thank You !!