using ApplicationCore.Requests.Activity;
using FitnessApp_Domain.Models.Activity;
using FitnessApp_Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.IServices
{
    public interface IActivityService
    {
        PaginationResponse<ActivityDto> GetAllActivity(GetAllActivityRequest request);
        PaginationResponse<ActivityGoalDto> GetAllActivityGoal(GetAllActivityGoalRequest request);

        ActivityDto GetById(GetByIdActivityRequest request);

        ActivityDto Create(CreateActivityRequest request);
        ActivityDto Update(UpdateActivityRequest request);
        void Delete(DeleteActivityRequest request);
    }
}
