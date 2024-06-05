using Fitness_WebCore.IServices;
using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Activity.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fitness_WebApplication.Areas.Activity.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly IActivityService _activityService;

        public CreateModel(IActivityService activityService)
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
        }
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            IList<SelectListItem> list = Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            Types = new SelectList(list, "Value", "Text");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                      kvp => kvp.Key,
                      kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                  );
                return new JsonResult(new { success = false, message = "Check input data!", errors });
            }

            var activity = new CreateActivity
            {
                Name = Input.Name,
                Description = Input.Description,
                Duration = Input.Duration,
                ActivityType = Input.ActivityType   
            };
            var result = _activityService.CreateActivity(activity);
            if (result.IsSuccess)
            {
                return new JsonResult(new { success = true, message = "Successfully created." });
            }
            return new JsonResult(new { success = false, message = "Failed to create new activity with given inputs." });
        }
    }
}
