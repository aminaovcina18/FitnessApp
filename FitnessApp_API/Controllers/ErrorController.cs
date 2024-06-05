using ApplicationCore.Helpers.Error;
using FitnessApp_Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp_API.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger logger)
        {
            _logger = logger;
        }

        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context!.Error;
            var errResponse = new ErrorResponse(exception);

            var code = ErrorHandlerHelper.ReturnStatusCode(exception);
            Response.StatusCode = code;
            errResponse.Status = code;

            _logger.LogError($"==={DateTime.Now:dd.MM.yyyy. HH:mm:ss}h===============================================");
            _logger.LogError(exception.Message);
            _logger.LogError(exception.StackTrace);
            _logger.LogError("=======================================================================================");

            return errResponse;
        }
    }
}
