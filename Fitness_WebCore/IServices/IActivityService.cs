using Fitness_WebCore.Helpers;
using FitnessApp_Domain.Models.Account;
using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Activity;
using FitnessApp_Domain.Models.Activity.RequestModels;
using FitnessApp_Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_WebCore.IServices
{
    public interface IActivityService
    {
        public HttpClientResponse<PaginationResponse<ActivityDto>> GetAll(string? activityType, DateTime? date, string? search, int page = 1, int pageSize = 10);
        public HttpClientResponse<PaginationResponse<ActivityGoalDto>> GetAllGoal(int page = 1, int pageSize = 10);
        public HttpClientResponse<ActivityDto> GetActivityById(int id);
        public HttpClientResponse<ActivityDto> CreateActivity(CreateActivity activity);
        public HttpClientResponse<ActivityDto> UpdateActivity(int id, UpdateActivity activity);
        public bool DeleteActivity(int id);
    }
}
