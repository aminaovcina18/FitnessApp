using ApplicationCore.Requests.Account;
using FitnessApp_Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.IServices
{
    public interface IAccountService
    {
        Task<UsersDto> CreateUser(CreateUserRequest request);

        UsersDto UpdateUser(UpdateUserRequest request);

        UsersDto GetCurrentUser(Guid userId);
    }
}
