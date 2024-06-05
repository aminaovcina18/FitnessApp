using IdentityModel;
using IdentityServer.Constants;
using IdentityServer4.Models;
using System.Security.Claims;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.IdentitySpecific
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", new[] { "role" })
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                    {
                        ClientId = IdentityConstants.WebClientId,
                        ClientName = "web",
                        ClientSecrets = { new Secret(IdentityConstants.WebClientSecret.Sha256()) },

                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AllowedScopes = { IdentityConstants.FitnessScope, StandardScopes.OpenId, StandardScopes.OfflineAccess, "roles" },
                        AllowOfflineAccess = true,
                        AccessTokenLifetime = 3600,
                        RefreshTokenExpiration = TokenExpiration.Absolute,
                        RefreshTokenUsage = TokenUsage.OneTimeOnly,
                        AbsoluteRefreshTokenLifetime = 1296000,

                        AlwaysIncludeUserClaimsInIdToken = true,
                        AccessTokenType = AccessTokenType.Reference
                    }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope(IdentityConstants.FitnessScope, "Fitness API"),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                // must match client beacuse of reference token
                new ApiResource
                {
                    Name = IdentityConstants.WebClientId,
                    ApiSecrets = { new Secret(IdentityConstants.WebClientSecret.Sha256()) },
                    Scopes = { IdentityConstants.FitnessScope, StandardScopes.OpenId, StandardScopes.OfflineAccess }
                },
            };
    }
}