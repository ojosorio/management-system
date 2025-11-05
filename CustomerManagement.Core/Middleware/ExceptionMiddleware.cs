using CustomerManagement.Core.Shared;
using CustomerManagement.Core.Shared.Helpers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CustomerManagement.Core.Middleware;

public class ExceptionMiddleware(IConfiguration configuration) : IMiddleware
{
    //private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IConfiguration _configuration = configuration;

    readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(e, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var (statusCode, response) = GetResponseForException(exception);
        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, jsonSerializerOptions);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(jsonResponse);
    }

    private (int StatusCode, StandardResponse<object> Response) GetResponseForException(Exception exception)
    {
        return exception switch
        {
            ValidationException ex => (
                StatusCodes.Status400BadRequest,
                CreateValidationErrorResponse(ex.Errors)
            ),
            UnauthorizedAccessException ex => (
                StatusCodes.Status403Forbidden,
                CreateForbiddenErrorResponse(ex.Message)
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                CreateInternalErrorResponse(exception)
            )
        };
    }

    private StandardResponse<object> CreateValidationErrorResponse(IEnumerable<ValidationFailure> errors)
    {
        List<string> validationErrors = [.. errors.Select(e => e.ErrorMessage)];
        //_logger.LogError("Validation failed: {Errors}", string.Join(", ", errors.Select(e => e.ErrorMessage)));

        var response = new StandardResponse<object>();
        response.SetBadRequestResponse(validationErrors);
        return response;
    }

    private static StandardResponse<object> CreateForbiddenErrorResponse(string message)
    {
        var response = new StandardResponse<object>();
        response.SetForbiddenResponse([message]);
        return response;
    }

    private StandardResponse<object> CreateInternalErrorResponse(Exception e)
    {
        var response = new StandardResponse<object>();
        var environment = _configuration["Config:Environment"];
        //_logger.LogError("Internal server error: {Message}\n{StackTrace}", e.Message, e.StackTrace);

        if (environment != "dev")
        {
            response.SetInternalServerErrorResponse(["An error has occurred on the server."]);
            return response;
        }

        response.SetInternalServerErrorResponse(["An error has occurred on the server.\n" + e.Message + "\n" + e.StackTrace]);
        return response;
    }

}
