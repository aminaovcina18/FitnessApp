using Fitness_WebCore.IServices;
using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Models.Activity.RequestModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Fitness_WebApplication.Areas.Activity.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class DetailsModel : PageModel
    {
        private readonly IActivityService _activityService;

        public DetailsModel(IActivityService activityService)
        {
            _activityService = activityService;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public SelectList Types { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Input required.")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Input required")]
            public string Description { get; set; }
            [Required(ErrorMessage = "Input required")]
            public int Duration { get; set; }
            [Required(ErrorMessage = "Selection required")]
            public ActivityType ActivityType { get; set; }
            [Required(ErrorMessage = "Input required")]
            public DateTime Date { get; set; }
        }
        public async Task<IActionResult> OnGetAsync(int id, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            IList<SelectListItem> list = Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            Types = new SelectList(list, "Value", "Text");
            var result = _activityService.GetActivityById(id);
            if (result.IsSuccess)
            {
                Input = new InputModel
                {
                    Name = result.Data.Name,
                    Description = result.Data.Description,
                    Date = result.Data.Date,
                    Duration = result.Data.Duration,
                    ActivityType = result.Data.ActivityType
                };
            }
            else
            {
                return LocalRedirect("/Index?message=Access denied");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                      kvp => kvp.Key,
                      kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                  );
                return new JsonResult(new { success = false, message = "Check input data!", errors });
            }

            var activity = new UpdateActivity
            {
                Name = Input.Name,
                Description = Input.Description,
                Duration = Input.Duration,
                ActivityType = Input.ActivityType,
                Date = Input.Date
            };
            var result = _activityService.UpdateActivity(id, activity);
            if (result.IsSuccess)
            {
                return new JsonResult(new { success = true, message = "Successfully updated." });
            }
            return new JsonResult(new { success = false, message = "Failed to update activity with given inputs." });
        }
    }
}

