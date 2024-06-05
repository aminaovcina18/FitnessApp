using Fitness_WebCore.IServices;
using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessApp_Domain.Models.Activity.RequestModels;
using FitnessApp_Domain.Models.Activity;
using Fitness_WebCore.Helpers;
using FitnessApp_Domain.Utils;

namespace Fitness_WebCore.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IHttpClientService _httpClientService;

        public ActivityService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public HttpClientResponse<PaginationResponse<ActivityDto>> GetAll(string? activityType, DateTime? date, string? search, int page = 1, int pageSize = 10 )
        {
            return _httpClientService.Get<PaginationResponse<ActivityDto>>($"activity/GetAll?page={page}&pageSize={pageSize}&search={search}&date={(date.HasValue ? date.Value.ToString("MM/dd/yyyy") : date)}&activityType={activityType}");
        }
        public HttpClientResponse<PaginationResponse<ActivityGoalDto>> GetAllGoal(int page = 1, int pageSize = 10)
        {
            return _httpClientService.Get<PaginationResponse<ActivityGoalDto>>($"activity/GetAllGoal?page={page}&pageSize={pageSize}");
        }
        public HttpClientResponse<ActivityDto> GetActivityById(int id)
        {
            return _httpClientService.Get<ActivityDto>($"activity/{id}");
        }
        public HttpClientResponse<ActivityDto> CreateActivity(CreateActivity activity)
        {
            return _httpClientService.Post<ActivityDto, CreateActivity>($"activity", activity);
        }

        public HttpClientResponse<ActivityDto> UpdateActivity(int id, UpdateActivity activity)
        {
            return _httpClientService.Patch<ActivityDto, UpdateActivity>($"activity/{id}", activity);
        }
        public bool DeleteActivity(int id)
        {
            return _httpClientService.Delete<bool>($"activity/{id}").IsSuccess;
        }

    }
}
