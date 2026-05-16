namespace Hl7ToFhirDemo.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("x-api-key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "API Key missing"
                });

                return;
            }

            var apiKey = _configuration["ApiSettings:ApiKey"];

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Unauthorized"
                });

                return;
            }

            await _next(context);
        }
    }
}
