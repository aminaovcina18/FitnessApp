using Fitness_WebCore.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Fitness_WebApplication.Areas.Identity.Pages
{
    [AllowAnonymous]
    [IgnoreAntiforgeryToken]
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IIdentityService _identityService;

        public LoginModel(IAccountService accountService,
            IIdentityService identityService)
        {
            _accountService = accountService;
            _identityService = identityService;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "ErrorUsername")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "ErrorPassword")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }
        public async Task<IActionResult> OnGetAsync(string returnUrl = null, string message = null)
        {
            ViewData["message"] = message;

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // if user is logged already, forward him to respected site
            var loggedInUser = await HttpContext.AuthenticateAsync();
            if (loggedInUser.Succeeded)
            {
                // attempt to refresh token
                await _identityService.RefreshToken(HttpContext.User.FindFirstValue("refresh_token"));
                return LocalRedirect("/Index");
            }

            // clean singIn cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            // clean singIn cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (ModelState.IsValid)
            {
                var principal = await _identityService.ValidateUserAndReturnClaims(Input.UserName, Input.Password);
                if (principal is null)
                {
                        
                    ErrorMessage = "Login failed.";
  
                    return RedirectToPage("./Login");
                }
                returnUrl =  "/Index";
                // sing in user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true,
                        ExpiresUtc = DateTime.Now.AddDays(15)
                    });
                return LocalRedirect(returnUrl);
            }

            ErrorMessage = "Username and password is required.";
            return RedirectToPage("./Login");
        }
    }
}
