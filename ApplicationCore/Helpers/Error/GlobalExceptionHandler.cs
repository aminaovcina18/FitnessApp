using FitnessApp_Domain.Utils;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ApplicationCore.Helpers.Error
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var result = new ProblemDetails();
            switch (exception)
            {
                case ArgumentNullException argumentNullException:
                    result = new ProblemDetails
                    {
                        Type = argumentNullException.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = argumentNullException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(argumentNullException, $"Exception occured : {argumentNullException.Message}");
                    break;
                case KeyNotFoundException keyNotFoundException:
                    result = new ProblemDetails
                    {
                        Type = keyNotFoundException.GetType().Name,
                        Title = "Unable to find object with given key",
                        Detail = keyNotFoundException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(keyNotFoundException, $"Exception occured : {keyNotFoundException.Message}");
                    break;
                case HttpStatusException httpStatusException:
                    result = new ProblemDetails
                    {
                        Type = httpStatusException.GetType().Name,
                        Title = "Internal error ocurred",
                        Detail = httpStatusException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(httpStatusException, $"Exception occured : {httpStatusException.Message}");
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    result = new ProblemDetails
                    {
                        Type = unauthorizedAccessException.GetType().Name,
                        Title = "Unauthorized access error ocurred",
                        Detail = unauthorizedAccessException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(unauthorizedAccessException, $"Exception occured : {unauthorizedAccessException.Message}");
                    break;
                case InvalidOperationException invalidOperationException:
                    result = new ProblemDetails
                    {
                        Type = invalidOperationException.GetType().Name,
                        Title = "Invalid operation error ocurred",
                        Detail = invalidOperationException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(invalidOperationException, $"Exception occured : {invalidOperationException.Message}");
                    break;
                case ArgumentException argumentException:
                    result = new ProblemDetails
                    {
                        Type = argumentException.GetType().Name,
                        Title = "Not all arguments provided",
                        Detail = argumentException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(argumentException, $"Exception occured : {argumentException.Message}");
                    break;
                default:
                    result = new ProblemDetails
                    {
                        Type = exception.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = exception.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                    };
                    _logger.LogError(exception, $"Exception occured : {exception.Message}");
                    break;
            }
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
            return true;
        }
    }
}
