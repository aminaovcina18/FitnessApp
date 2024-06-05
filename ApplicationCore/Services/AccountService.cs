using ApplicationCore.IRepositories;
using ApplicationCore.IServices;
using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FitnessApp_Domain.Mappers;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ApplicationCore.Requests.Account;
using Microsoft.AspNetCore.Identity;
using System.Net;
using FitnessApp_Domain.Utils;
using FitnessApp_Domain.Models.Common;

namespace ApplicationCore.Services
{
    public class AccountService : IAccountService
    {
        public readonly IRepository<Users> _appUserRepository;
        public readonly IRepository<Roles> _appRoleRepository;
        private readonly UserManager<Users> _userManager;
        public AccountService(
            UserManager<Users> userManager,
           IRepository<Users> appUserRepository,
           IRepository<Roles> appRoleRepository
           )
        {
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _appRoleRepository = appRoleRepository;
        }
        public UsersDto GetCurrentUser(Guid userId)
        {
            Expression<Func<Users, bool>> criteria = e => e.Id == userId;
            var user = this._appUserRepository.GetWithInclude(criteria, new string[] { "UserRoles.Role" });
            if (user is null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return user.ToDto();
        }
        public UsersDto UpdateUser(UpdateUserRequest request)
        {
            Expression<Func<Users, bool>> criteria = e => e.Id == request.UserId;
            var entity = _appUserRepository.GetWithInclude(criteria, new string[] { "UserRoles.Role" });
            if (entity is null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            entity.FirstName = request.FirstName ?? entity.FirstName;
            entity.LastName = request.LastName ?? entity.LastName;
            entity.PhoneNumber = request.PhoneNumber ?? entity.PhoneNumber;
            entity.Active = request.Active ?? entity.Active;
            entity.ActivityCount = request.ActivityCount;
            entity.ActivityDuration = request.ActivityDuration;

            _appUserRepository.Update(entity);

            return entity.ToDto();
        }
        public async Task<UsersDto> CreateUser(CreateUserRequest request)
        {
            if (System.String.IsNullOrEmpty(request.Email))
            {
                throw new ArgumentNullException("Email", "Email is required.");
            }

            Expression<Func<Users, bool>> criteria = e => e.Email == request.Email || e.UserName == request.Username 
                || (!System.String.IsNullOrEmpty(request.PhoneNumber) && e.PhoneNumber == request.PhoneNumber);
            var existingUser = _appUserRepository.GetWithInclude(criteria);

            if (existingUser is not null)
            {
                throw new ArgumentException("User already exists.");
            }

            var user = new Users
            {
                UserName = request.Username,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Active = true,
                ApprovedByAdmin = true,
                EmailConfirmed = false,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ActivityCount = request.ActivityCount,
                ActivityDuration = request.ActivityDuration,
                CreatedOn = DateTime.Now,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new HttpStatusException(HttpStatusCode.InternalServerError, "An error ocurred during user registration.");
            }

            await _userManager.AddToRoleAsync(user, RoleEnum.REGULAR.ToString());

            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new HttpStatusException(HttpStatusCode.InternalServerError, "An error ocurred during user registration.");
            }

            return user.ToDto();
        }
    }
}
