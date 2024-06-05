using Fitness_WebCore.Helpers;
using Fitness_WebCore.Services;
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

namespace Fitness_WebCore.IServices
{
    public interface IIdentityService
    {
        Task<ClaimsPrincipal> ValidateUserAndReturnClaims(string username, string password);
        Task<TokenIntrospect> GetTokenData(string token);
        Task RefreshToken(string refresh_token);
        Task<bool> RevokeToken(string token);

    }
}
