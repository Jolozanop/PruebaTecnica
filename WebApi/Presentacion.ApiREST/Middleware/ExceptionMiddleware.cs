using Serilog;
using System.Net;
using System.Text.Json;

namespace Presentacion.ApiREST.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, ex.InnerException != null ? ex.InnerException.Message : "");
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                Log.Information($"Error: {ex.Message} - {(ex.InnerException != null ? ex.InnerException.Message : "")}");

                var response = _env.IsDevelopment() ? new CodigoErrorException((int)HttpStatusCode.InternalServerError, ex.Message, ex.InnerException != null ? ex.InnerException.Message : "")
                    : new CodigoErrorException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
