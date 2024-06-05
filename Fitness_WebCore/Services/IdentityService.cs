using Fitness_WebCore.Helpers;
using Fitness_WebCore.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Fitness_WebCore.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityConfig _identityConstants;
        private readonly BaseUrl _baseUri;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IOptions<IdentityConfig> identityConstants,
                        IOptions<BaseUrl> baseUri,
                        IHttpContextAccessor httpContextAccessor,
                        ILogger<IdentityService> logger)
        {
            _identityConstants = identityConstants.Value;
            _baseUri = baseUri.Value;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ClaimsPrincipal> ValidateUserAndReturnClaims(string username, string password)
        {
            // password verification
            _identityConstants.Username = username;
            _identityConstants.Password = password;
            _identityConstants.Grant_Type = "password";
            HttpResponseMessage response = await SendHttpPostRequest(_identityConstants, "connect/token");

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<AccessToken>();
                return await CreateUserPrincipal(token);
            }

            return null;
        }

        public async Task RefreshToken(string refresh_token)
        {

            _identityConstants.Grant_Type = "refresh_token";
            _identityConstants.Refresh_Token = refresh_token;
            HttpResponseMessage response = await SendHttpPostRequest(_identityConstants, "connect/token");

            // replace claims with new ones on success
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadFromJsonAsync<AccessToken>();
                var principal = await CreateUserPrincipal(token);
                if (principal is null)
                    throw new Exception("Failed to fetch user claims");

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            new AuthenticationProperties()
                            {
                                AllowRefresh = true,
                                IsPersistent = true,
                                ExpiresUtc = DateTime.Now.AddDays(15)
                            });
            }
            else
            {
                throw new Exception("Failed to refresh token");
                //baci me na login
            }
        }

        public async Task<TokenIntrospect> GetTokenData(string token)
        {
            _identityConstants.Token = token;
            HttpResponseMessage response = await SendHttpPostRequest(_identityConstants, "connect/introspect");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<TokenIntrospect>();
            }

            return null;
        }

        public async Task<bool> RevokeToken(string token)
        {
            _identityConstants.Token = token;
            HttpResponseMessage response = await SendHttpPostRequest(_identityConstants, "connect/revocation");

            return response.IsSuccessStatusCode;
        }

        // calling through httpclient since parsing is different for claims
        private async Task<ClaimsPrincipal> CreateUserPrincipal(AccessToken token)
        {
            var tokenData = await GetTokenData(token.Access_Token);
            if (tokenData is not null)
            {
                // store new access token in Context items, that will live until request ends
                _httpContextAccessor.HttpContext.Items["access_token"] = token.Access_Token;

                // store global/user information in claims
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenData.Sub),
                    new Claim("access_token", token.Access_Token),
                    //new Claim("refresh_token", token.Refresh_Token)
                };

                // role claims
                if (tokenData.Role is not null)
                {
                    var roles = tokenData.Role.Split(",");
                    for (int i = 0; i < roles.Length; i++)
                        claims.Add(new Claim(ClaimTypes.Role, roles[i]));
                }

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return new ClaimsPrincipal(identity);
            }

            return null;
        }

        private async Task<HttpResponseMessage> SendHttpPostRequest(IdentityConfig config, string requestUri)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(_baseUri.IdentityUri)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(config);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var response = await client.PostAsync(requestUri, new FormUrlEncodedContent(dictionary));
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(); // raw content as string
                _logger.LogError($"Url: {_baseUri.IdentityUri}/{requestUri} - StatusCode: {response.StatusCode} - ErrorMessage: {content} ");
            }

            return response;
        }
    }
}
