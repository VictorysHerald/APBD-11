using System.Data;

namespace APBD_10.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
 
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            context.Response.StatusCode = ex is DataException ? 404 : 500;
        }
    }
}