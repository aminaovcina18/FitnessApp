using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Mappers
{
    public static class AccountMapper
    {
        public static UsersDto ToDto(this Users entity)
        {
            return new UsersDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                Roles = entity.UserRoles?.Select(s => s.Role.Name).ToList(),
                CreatedOn = entity.CreatedOn,
                Active = entity.Active,
                ActivityCount = entity.ActivityCount,
                ActivityDuration = entity.ActivityDuration
            };
        }
    }
}
