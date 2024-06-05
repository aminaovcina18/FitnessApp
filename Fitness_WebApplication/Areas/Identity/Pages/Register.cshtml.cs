using Fitness_WebCore.IServices;
using FitnessApp_Domain.Models.Account.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Fitness_WebApplication.Areas.Identity.Pages
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public RegisterModel(IAccountService accountService)
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
            [Required(ErrorMessage = "Input required.")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Input required")]
            public string Firstname { get; set; }
            [Required(ErrorMessage = "Input required")]
            public string Lastname { get; set; }
            [Required(ErrorMessage = "Input required.")]
            [EmailAddress(ErrorMessage = "Invalid format.")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Input required.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Input required.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$#!%*?&])[A-Za-z\d@$#!%*?&]{8,}$", ErrorMessage = "PasswordRegex")]
            [StringLength(100, ErrorMessage = "Password policy not satisfied.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Required(ErrorMessage = "Input required.")]
            [Compare("Password", ErrorMessage = "Passwords don't match.")]
            [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }

        }
        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
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

            var user = new CreateUser
            {
                Username = Input.Username,
                Email = Input.Email,
                Password = Input.Password,
                FirstName = Input.Firstname,
                LastName = Input.Lastname,
                PhoneNumber = Input.PhoneNumber
            };
            var result = _accountService.CreateUser(user);
            if (result.IsSuccess)
            {
                return new JsonResult(new { success = true, message = "Registration successful." });
            }
            return new JsonResult(new { success = false, message = "Failed to register new user with given inputs." });
        }
    }

}
