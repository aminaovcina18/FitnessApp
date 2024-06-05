using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Domain.Models.Account;
using FitnessApp_Domain.Models.Activity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp_Domain.Mappers
{
    public static class ActivityMapper
    {
        public static ActivityDto ToDto(this Activity entity)
        {
            return new ActivityDto
            {
                Id = entity.Id,
                Duration = entity.Duration,
                ActivityType = entity.ActivityType,
                Description = entity.Description,
                IsDeleted = entity.IsDeleted,
                Date = entity.Date,
                UserId = entity.UserId,
                Name = entity.Name,
                FirstName = entity!.User!.FirstName,
                LastName = entity!.User!.LastName
            };
        }
    }
}
