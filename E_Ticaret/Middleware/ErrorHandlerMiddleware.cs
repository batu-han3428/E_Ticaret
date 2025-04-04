using E_Ticaret.Exceptions;
using E_Ticaret.Services.Interfaces;
using Newtonsoft.Json;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            AppException appEx => appEx.StatusCode,
            _ => 500
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new
        {
            message = ex.Message,
            details = ex.InnerException?.Message
        };

        _logger.LogError("An error occurred", ex);

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
