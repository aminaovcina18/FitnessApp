using Fitness_WebCore.IServices;
using FitnessApp_Domain.Models.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fitness_WebApplication.Areas.Activity.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class GoalListModel : PageModel
    {
        private readonly IActivityService _activityService;

        public GoalListModel(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public List<ActivityGoalDto> TableData { get; set; }
        public int Page { get; set; }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            var result = _activityService.GetAllGoal(pageNumber, pageSize);
            if (result.IsSuccess)
            {
                TableData = result.Data.Data.ToList();
                Page = result.Data.Page;
            }
            else
            {
                return LocalRedirect("/Index?message=Goal not set");
            }
            return Page();
        }
        public async Task<IActionResult> OnGetPaginationAsync(int pageNumber = 1, int pageSize = 10)
        {
            var result = _activityService.GetAllGoal(pageNumber, pageSize);
            if (result.IsSuccess)
            {
                TableData = result.Data.Data.ToList();
                Page = result.Data.Page;
            }
            else
            {
                return LocalRedirect("/Index?message=Goal not set");
            }
            return new JsonResult(new { success = true, data = TableData, page = Page });
        }
    }
}
