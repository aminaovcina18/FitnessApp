using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using FitnessApp_Domain.Entities.Helper;
using FitnessApp_Domain.Entities.Identity;
using System.Security.Claims;
using IdentityModel;
using System.Data;

namespace IdentityServer.IdentitySpecific
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<Users> _userManager;
        private readonly UserClaimsPrincipalFactory<Users, Roles> _userClaimsPrincipalFactory;

        public CustomProfileService(UserManager<Users> userManager, UserClaimsPrincipalFactory<Users, Roles> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            var userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            if (user == null)
                return;

            var claims = userClaims.Claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
            if (user != null)
                context.IsActive = user.Active;
        }
    }
}
