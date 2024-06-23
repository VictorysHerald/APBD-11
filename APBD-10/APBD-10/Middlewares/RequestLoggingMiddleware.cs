namespace APBD_10.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);

        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            _logger.LogInformation("Token: {Token}", token);
        }
        else
        {
            _logger.LogWarning("No Authorization header found.");
        }

        await _next(context);

        _logger.LogInformation("Finished handling request.");
    }
}