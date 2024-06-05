using FitnessApp_Domain.Entities.Fitness;
using FitnessApp_Domain.Models.Activity.RequestModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Fitness_WebCore.IServices;
using FitnessApp_Domain.Models.Account.RequestModels;

namespace Fitness_WebApplication.Areas.Account.Pages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class DetailsModel : PageModel
    {
        private readonly IAccountService _accountService;

        public DetailsModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Input required")]
            public string Firstname { get; set; }
            [Required(ErrorMessage = "Input required")]
            public string Lastname { get; set; }
            [Required(ErrorMessage = "Input required.")]
            public string PhoneNumber { get; set; }
            public bool IsCountSelected { get; set; }
            public int? GoalValue { get; set; }


        }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
          
            var result = _accountService.GetMe();
            if (result.IsSuccess)
            {
                Input = new InputModel
                {
                    Firstname = result.Data.FirstName,
                    Lastname = result.Data.LastName,
                    PhoneNumber = result.Data.PhoneNumber,
                    GoalValue = (result.Data.ActivityCount != null && result.Data.ActivityCount != 0) ? result.Data.ActivityCount : result.Data.ActivityDuration,
                    IsCountSelected = (result.Data.ActivityCount != null && result.Data.ActivityCount != 0) ? true : false
                };
            }
            else
            {
                return LocalRedirect("/Index?message=Access denied");
            }
            return Page();
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

            var user = new UpdateUser
            {
                FirstName = Input.Firstname,
                LastName = Input.Lastname,
                PhoneNumber = Input.PhoneNumber,
                ActivityDuration = Input.IsCountSelected ? null : Input.GoalValue,
                ActivityCount = Input.IsCountSelected ? Input.GoalValue : null,
            };
            var result = _accountService.UpdateUser(user);
            if (result.IsSuccess)
            {
                return new JsonResult(new { success = true, message = "Successfully updated." });
            }
            return new JsonResult(new { success = false, message = "Failed to update activity with given inputs." });
        }
    }
}

