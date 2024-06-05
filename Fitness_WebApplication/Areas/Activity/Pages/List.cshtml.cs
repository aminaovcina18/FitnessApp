using Fitness_WebCore.IServices;
using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Entities.Identity;
using FitnessApp_Domain.Models.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fitness_WebApplication.Areas.Activity.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class ListModel : PageModel
    {
        private readonly IActivityService _activityService;

        public ListModel(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public List<ActivityDto> TableData { get; set; }
        public int Page { get; set; }
        public int NumberOfPages { get; set; }
        public SelectList Types { get; set; }

        public async Task OnGetAsync(string? activitytype = null, DateTime? date = null, string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            IList<SelectListItem> list = Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            Types = new SelectList(list, "Value", "Text");
            var result = _activityService.GetAll(activitytype, date, search, pageNumber, pageSize);
            if (result.IsSuccess)
            {
                TableData = result.Data.Data.ToList();
                Page = result.Data.Page;
                NumberOfPages = result.Data.Pages + 1;
            }
        }
        public async Task<IActionResult> OnGetLoadDataAsync(string? activitytype = null, DateTime? date = null, string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            var result = _activityService.GetAll(activitytype, date, search, pageNumber, pageSize);
            if (result.IsSuccess)
            {
                TableData = result.Data.Data.ToList();
                Page = result.Data.Page;
                NumberOfPages = result.Data.Pages + 1;
            }

            return Partial("_List", TableData);
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = _activityService.DeleteActivity(id);
            if (result)
            {
                return new JsonResult(new { success = true, message = "Successfully deleted." });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Error while deleting." });
            }
        }
    }
}
