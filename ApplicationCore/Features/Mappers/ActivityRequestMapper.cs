using ApplicationCore.Requests.Account;
using ApplicationCore.Requests.Activity;
using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Activity.RequestModels;
using FitnessApp_Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Features.Mappers
{
    public static class ActivityRequestMapper
    {
        public static CreateActivityRequest ToRequest(this CreateActivity request, Guid createdBy)
        {
            return new CreateActivityRequest
            {
               Name = request.Name,
               Description = request.Description,
               Duration = request.Duration,
               ActivityType = request.ActivityType,
               UserId = createdBy
            };
        }

        public static UpdateActivityRequest ToRequest(this UpdateActivity request, int id, Guid updatedBy)
        {
            return new UpdateActivityRequest
            {
                Name = request.Name,
                Description = request.Description,
                Duration = request.Duration,
                ActivityType = request.ActivityType,
                UpdatedBy = updatedBy,
                Id = id,
                Date = request.Date
            };
        }

        public static GetAllActivityGoalRequest ToRequest(this GetAllActivityGoal request, Guid userId)
        {
            return new GetAllActivityGoalRequest
            {
                UserId = userId,
                Page = request.Page,
                PageSize = request.PageSize,
                OrderBy = request.OrderBy,
                OrderByDirection = !String.IsNullOrEmpty(request.OrderByDirection) ? Enum.Parse<OrderByDirectionType>(request.OrderByDirection) : OrderByDirectionType.DESC
            };
        }
        public static GetAllActivityRequest ToRequest(this GetAllActivity request, Guid userId)
        {
            return new GetAllActivityRequest
            {
                UserId = userId,
                Page = request.Page,
                PageSize = request.PageSize,
                OrderBy = request.OrderBy,
                OrderByDirection = !String.IsNullOrEmpty(request.OrderByDirection) ? Enum.Parse<OrderByDirectionType>(request.OrderByDirection) : OrderByDirectionType.DESC,
                Search = request.Search,
                Date = request.Date,
                ActivityType = !String.IsNullOrEmpty(request.ActivityType) ? Enum.Parse<ActivityType>(request.ActivityType) : null
            };
        }
    }
}
