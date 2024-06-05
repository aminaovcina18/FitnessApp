using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_WebCore.Helpers;

namespace Fitness_WebCore.IServices
{
    public interface IAccountService
    {
        public HttpClientResponse<UsersDto> GetMe();
        public HttpClientResponse<UsersDto> CreateUser(CreateUser user);
        public HttpClientResponse<UsersDto> UpdateUser(UpdateUser user);
    }
}
