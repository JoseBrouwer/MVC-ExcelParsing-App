using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExcelParsing.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next; //will be called any time there's an HTTP request, returns the completed Task
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); //intercept HTTP requests, passes context if Task completes
            }
            catch (Exception ex) //Task failed, unhandled
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Redirect("/ExcelParsing/Error");
            return Task.CompletedTask;
        }
    }
}
