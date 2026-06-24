using System.Text.Json;
using BookReadingEvents.Application.Exceptions;
namespace BookReadingEvents.API.Middleware;

public class ExceptionHandlingMiddleware  // the problem it solves is that it converts C# exceptions into standardized HTTP responses with appropriate status codes and error messages, improving the robustness and user-friendliness of the API. It ensures that clients receive consistent and informative error responses, regardless of where the exception occurred in the application.
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
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
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType =
            "application/json";

        context.Response.StatusCode =
            exception switch  // switch expression used in modern c#
            {
                NotFoundException =>
                    StatusCodes.Status404NotFound,

                ForbiddenException =>
                    StatusCodes.Status403Forbidden,

                BusinessRuleException =>
                    StatusCodes.Status400BadRequest,

                _ =>
                    StatusCodes.Status500InternalServerError
            };

        var response =
            new
            {
                StatusCode =
                    context.Response.StatusCode,

                Message =
                    exception.Message
            };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}