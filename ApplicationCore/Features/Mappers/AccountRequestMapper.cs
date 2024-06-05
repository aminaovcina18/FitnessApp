using ApplicationCore.Requests.Account;
using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Features.Mappers
{
    public static class AccountRequestMapper
    {

        public static CreateUserRequest ToRequest(this CreateUser request)
        {
            return new CreateUserRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Username = request.Username,
                Password = request.Password,
                ActivityDuration = request.ActivityDuration,
                ActivityCount = request.ActivityCount
            };
        }

        public static UpdateUserRequest ToRequest(this UpdateUser request, Guid userId, Guid updatedBy)
        {
            return new UpdateUserRequest
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UpdatedBy = updatedBy,
                ActivityDuration = request.ActivityDuration,
                ActivityCount = request.ActivityCount,
                Active = request.Active
            };
        }


    }
}
