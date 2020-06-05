using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var discoveryResponse = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (discoveryResponse.IsError)
            {
                Console.WriteLine(discoveryResponse.Error);
                return;
            }

            await ROpasswordAndClientCredentialsExample(client, discoveryResponse);

            var resourceOwnerPasswordResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,

                ClientId = "ResourceOwnerPassword",
                ClientSecret = "secret",
                GrantType = "password",
                UserName = "alice",
                Password = "alice",
                Scope = "profile"
            });

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            var validatedToken = Validate(tokenResponse.AccessToken);

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("http://localhost:6001/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }

        private static async Task ROpasswordAndClientCredentialsExample(HttpClient client,
            DiscoveryDocumentResponse discoveryResponse)
        {
            var resourceOwnerPasswordAndClientCredentialsResponse = await client.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,

                    ClientId = "ResourceOwnerPasswordAndClientCredentials",
                    ClientSecret = "secret",
                    GrantType = "password",
                    UserName = "alice",
                    Password = "alice",
                    Scope = "profile api1"
                });

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                resourceOwnerPasswordAndClientCredentialsResponse.AccessToken);
            var response1 = await httpClient.GetStringAsync("http://localhost:6001/identity");
        }

        private static JwtSecurityToken Validate(string tokenStr)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
                new RSAParameters()
                {
                    Modulus = Convert.FromBase64String("uta50Vcfti9AyqP9ws2MNT5B18FArO6DeG5vaNhUKWzL11cFdFcdyXLByyr6bfDXF6up4JOQCUYTc+sZAVFhS3y5FhE/Me83ZMEL0sD+tOEVst05dB+Ne1e+2G6kzJ2jxUTfUVV3Pqiepoxj2PKoXw9tZy9P2tEoCwiKKQOegROD2Mt/Pam0yAygOqOqfVQMVfhqp/fyPW4iYgzqK9OeaAx/leNuRdnwFcjjV+Ka9fNdR+fsIiGnQKs6Ana16/xJLIP8XNb/6hGi7UWKhMsQNWGi8V86pKXAi9N8zr/30rBKj6NKcmdGtBbaTtRBSKO9SZoYqH+rB07rO9JX77YFSQ=="),
                    Exponent = Convert.FromBase64String(("AQAB"))
                });

            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };

            SecurityToken validatedSecurityToken = null;
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(tokenStr, validationParameters, out validatedSecurityToken);
            JwtSecurityToken validatedJwt = validatedSecurityToken as JwtSecurityToken;
            return validatedJwt;
        }
    }
}
