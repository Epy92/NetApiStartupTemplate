using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Setup
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor accessor, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _accessor = accessor;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error was occurred during the process. Trace Identifier: {_accessor.HttpContext.TraceIdentifier}.");

                await HandleExceptionMessageAsync(_accessor.HttpContext).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context)
        {
            string response = JsonSerializer.Serialize(new ValidationProblemDetails()
            {
                Title = "An error was occurred.",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path,
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(response);
        }
    }
}
