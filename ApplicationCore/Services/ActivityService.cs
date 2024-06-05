using ApplicationCore.IRepositories;
using ApplicationCore.IServices;
using ApplicationCore.Requests.Activity;
using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Domain.Models.Activity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FitnessApp_Domain.Mappers;
using FitnessApp_Domain.Utils;
using System.Reflection.Metadata.Ecma335;

namespace ApplicationCore.Services
{
    public class ActivityService : IActivityService
    {
        public readonly IRepository<Activity> _appActivityRepository;
        private readonly IRepository<Users> _appUserRepository;
        public ActivityService(
            IRepository<Users> appUserRepository,
           IRepository<Activity> appActivityRepository
           )
        {
            _appUserRepository = appUserRepository;
            _appActivityRepository = appActivityRepository;
        }
        public ActivityDto GetById(GetByIdActivityRequest request)
        {
            Expression<Func<Activity, bool>> criteria = e => e.Id == request.ActivityId;
            var entity = _appActivityRepository.GetWithInclude(criteria, new string[] { "User" });
            if (entity is null)
            {
                throw new KeyNotFoundException("Aktivnost nije pronađena.");
            }
            if(entity.UserId != request.userId)
            {
                throw new UnauthorizedAccessException("Nemate pravo pristupa aktivnosti.");
            }
            return entity.ToDto();
        }
        public ActivityDto Create(CreateActivityRequest request)
        {
            Activity activity = new Activity
            {
                UserId = request.UserId,
                Name = request.Name,
                Description = request.Description,
                Date = DateTime.Now,
                Duration = request.Duration,
                ActivityType = request.ActivityType,
                CreatedBy = request.UserId
            };
            return _appActivityRepository.InsertWithReturn(activity).ToDto();
        }
        public ActivityDto Update(UpdateActivityRequest request)
        {
            Expression<Func<Activity, bool>> criteria = e => e.Id == request.Id;
            var entity = _appActivityRepository.GetWithInclude(criteria, new string[] { "User" });
            if (entity is null)
            {
                throw new KeyNotFoundException("Activity not found.");
            }
            if (entity.UserId != request.UpdatedBy)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this activity.");
            }
            entity.ActivityType = request.ActivityType ?? entity.ActivityType;
            entity.Duration = request.Duration ?? entity.Duration;
            entity.Date = request.Date ?? entity.Date;
            entity.Name = request.Name ?? entity.Name;
            entity.Description = request.Description ?? entity.Description;
            entity.UpdatedBy = request.UpdatedBy;
            _appActivityRepository.Update(entity);
            return entity.ToDto();
        }
        public void Delete(DeleteActivityRequest request)
        {
            Expression<Func<Activity, bool>> criteria = e => e.Id == request.ActivityId;
            var entity = _appActivityRepository.GetWithInclude(criteria);

            if (entity is null)
            {
                throw new KeyNotFoundException("Activity not found.");
            }
            if (entity.UserId != request.UpdatedBy)
            {
                throw new UnauthorizedAccessException("You are not authorized to access this activity.");
            }
            entity.IsDeleted = true;
            entity.UpdatedBy = request.UpdatedBy;
            _appActivityRepository.Update(entity);
        }
        public PaginationResponse<ActivityDto> GetAllActivity(GetAllActivityRequest request)
        {
            Expression<Func<Activity, bool>> criteria = e =>
            e.UserId == request.UserId
            && (!request.ActivityType.HasValue || e.ActivityType == request.ActivityType)
            && (!request.Date.HasValue || e.Date.Date == request.Date.Value.Date)
            &&
            (
            String.IsNullOrEmpty(request.Search) || e.Name.Contains(request.Search) || e.Description.Contains(request.Search)
            );
            var response = _appActivityRepository
            .GetPaginatedData(new GetPaginatedDataRequest<Activity>()
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Criteria = criteria,
                OrderBy = request.OrderBy,
                OrderByDirection = request.OrderByDirection.ToString(),
            });

            return new PaginationResponse<ActivityDto>
            {
                Data = response.Data.Select(s => s.ToDto()),
                Page = response.Page,
                Pages = response.Pages,
                Total = response.Total,
            };
        }
        public PaginationResponse<ActivityGoalDto> GetAllActivityGoal(GetAllActivityGoalRequest request)
        {
            var dateFrom = DateTime.Today.AddDays(-request.Page * request.PageSize + 1);
            var dateTo = DateTime.Today.AddDays(-request.Page * request.PageSize + request.PageSize);

            Expression<Func<Users, bool>> userCriteria = e => e.Id == request.UserId;
            var user = _appUserRepository.GetWithInclude(userCriteria);

            if(!user.ActivityCount.HasValue && !user.ActivityDuration.HasValue)
                throw new InvalidOperationException("Goal not set.");

            Expression<Func<Activity, bool>> criteria = e =>
            e.UserId == request.UserId && e.Date.Date >= dateFrom.Date && e.Date.Date <= dateTo.Date;

            var activities = _appActivityRepository.GetAllWithCriteria(criteria)
               .GroupBy(activity => activity.Date.Date)
               .ToDictionary(group => group.Key, group => group);

            var activityGoalDtoList = Enumerable.Range(0, (int)(dateTo - dateFrom).TotalDays + 1)
                .Select(offset =>
                {
                    var date = dateFrom.AddDays(offset);
                    var activityList = activities.Where(kvp => kvp.Key.Date == date.Date).SelectMany(kvp => kvp.Value).ToList();
                    if (user.ActivityCount.HasValue)
                    {
                        return new ActivityGoalDto
                        {
                            Date = date,
                            Achived = activityList.Any() ? activityList.Count() >= user.ActivityCount : false
                        };
                    }
                    else 
                    { 
                        return new ActivityGoalDto
                        {
                            Date = date,
                            Achived = activityList.Any() ? activityList.Sum(activity => activity.Duration) >= user.ActivityDuration : false
                        };
                    }
                }).OrderByDescending(x => x.Date);
            return new PaginationResponse<ActivityGoalDto>
            {
                Data = activityGoalDtoList,
                Page = request.Page,
            };
        }

    }
}
