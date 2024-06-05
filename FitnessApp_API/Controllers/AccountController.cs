using ApplicationCore.Features.Mappers;
using ApplicationCore.Helpers.Identity;
using ApplicationCore.IServices;
using ApplicationCore.Requests.Account;
using ApplicationCore.Requests.Activity;
using ApplicationCore.Services;
using FitnessApp_Domain.Models.Account.RequestModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessApp_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserClaimsUtil _userClaims;
        public AccountController(IAccountService accountService, IUserClaimsUtil userClaims)
        {
            _accountService = accountService;
            _userClaims = userClaims;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser request)
        {
            return Ok(await _accountService.CreateUser(request.ToRequest()));
        }
        [HttpPatch]
        [Authorize(Roles = "REGULAR, ADMIN")]
        public IActionResult UpdateUser([FromBody] UpdateUser request)
        {
            return Ok(_accountService.UpdateUser(request.ToRequest(_userClaims.UserId, _userClaims.UserId)));
        }
        [HttpGet("me")]
        [Authorize(Roles = "REGULAR")]
        public IActionResult GetMe()
        {
            return Ok(_accountService.GetCurrentUser(_userClaims.UserId));
        }
    }
}
