using Fitness_WebCore.Helpers;
using Fitness_WebCore.IServices;
using FitnessApp_Domain.Models.Account;
using FitnessApp_Domain.Models.Account.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpClientService _httpClientService;

        public AccountService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public HttpClientResponse<UsersDto> GetMe()
        {
            return _httpClientService.Get<UsersDto>($"account/me");
        }

        public HttpClientResponse<UsersDto> CreateUser(CreateUser user)
        {
            return _httpClientService.Post<UsersDto, CreateUser>($"account", user);
        }

        public HttpClientResponse<UsersDto> UpdateUser(UpdateUser user)
        {
            return _httpClientService.Patch<UsersDto, UpdateUser>($"account", user);
        }
    }
}
