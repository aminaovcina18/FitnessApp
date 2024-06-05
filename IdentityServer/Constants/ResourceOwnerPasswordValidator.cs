using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Infrastructure;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static IdentityModel.OidcConstants;

namespace IdentityServer.Constants
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;
        private readonly DBContext _dbContext;
        private readonly UserManager<Users> _userManager;

        public ResourceOwnerPasswordValidator(ILogger<ResourceOwnerPasswordValidator> logger,
                                              DBContext dbContext,
                                              UserManager<Users> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                Users? user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == context.UserName || x.Email == context.UserName);
                if (user is null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Korisničko ime ili password nisu tačni.");
                    return;
                }
                if (!user.Active)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidScope, "Korisnik sa ovim podacima nije aktivan. Molimo Vas da kontaktirate administratore.");
                    return;
                }

                // validate password
                if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, context.Password) == PasswordVerificationResult.Success)
                {
                    context.Result = new GrantValidationResult(
                        subject: user.Id.ToString(),
                        authenticationMethod: AuthenticationMethods.Password);

                    context.Result.CustomResponse = new Dictionary<string, object>
                        {
                            { "user_id", user.Id },
                            { "roles", await _userManager.GetRolesAsync(user) },
                        };
                }
                // if password validation fails
                else
                {
                    await UpdateUserDataOnFailedLogin(user);
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Korisničko ime ili password nisu tačni.");
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Unable to validate credentials: {JsonConvert.SerializeObject(e)}");
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Došlo je do greške prilikom prijave. Pokušajte ponovo ili kontaktirajte administratore.");
            }
        }

        // check if user has assigned role
        public async Task<bool> ValidateRoleAsync(string username, string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            if (users.FirstOrDefault(u => u.UserName == username) != null)
                return true;
            return false;
        }

        private async Task UpdateUserDataOnFailedLogin(Users user)
        {
            try
            {
                user.AccessFailedCount += 1;
                if (user.AccessFailedCount > 10)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.Now.AddHours(1);
                }

                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Update user data on failed login success for user: {user.UserName}");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update user data failed for user: {user.UserName}, reason: {e.Message}");
            }
        }

    }
}
