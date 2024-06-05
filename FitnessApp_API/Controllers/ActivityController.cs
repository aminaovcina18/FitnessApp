using ApplicationCore.Features.Mappers;
using ApplicationCore.Helpers.Identity;
using ApplicationCore.IServices;
using ApplicationCore.Requests.Activity;
using ApplicationCore.Services;
using FitnessApp_Domain.Models.Account.RequestModels;
using FitnessApp_Domain.Models.Activity.RequestModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessApp_API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/activity")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IUserClaimsUtil _userClaims;
        public ActivityController(IActivityService activityService, IUserClaimsUtil userClaims)
        {
            _activityService = activityService;
            _userClaims = userClaims;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "REGULAR")]
        public IActionResult GetAll([FromQuery] GetAllActivity request)
        {
            return Ok(_activityService.GetAllActivity(request.ToRequest(_userClaims.UserId)));
        }
        [HttpGet("GetAllGoal")]
        [Authorize(Roles = "REGULAR")]
        public IActionResult GetAllGoal([FromQuery] GetAllActivityGoal request)
        {
            return Ok(_activityService.GetAllActivityGoal(request.ToRequest(_userClaims.UserId)));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "REGULAR")]
        public IActionResult GetById(int id)
        {
            return Ok(_activityService.GetById(new GetByIdActivityRequest(id, _userClaims.UserId)));
        }
        [HttpPost]
        [Authorize(Roles = "REGULAR")]
        public IActionResult Create([FromBody] CreateActivity request)
        {
            return Ok(_activityService.Create(request.ToRequest(_userClaims.UserId)));
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "REGULAR")]
        public IActionResult Patch(int id, [FromBody] UpdateActivity request)
        {
            return Ok(_activityService.Update(request.ToRequest(id, _userClaims.UserId)));
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "REGULAR")]
        public IActionResult Delete(int id)
        {
            _activityService.Delete(new DeleteActivityRequest(id, _userClaims.UserId));
            return Ok();
        }
    }
}
