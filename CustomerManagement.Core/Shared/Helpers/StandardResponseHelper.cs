using FluentValidation.Results;
using System.Net;
using System.Runtime.CompilerServices;

namespace CustomerManagement.Core.Shared.Helpers;

public static class StandardResponseHelper
{
    #region StandarResponse

    public static void SetSuccessResponse(this StandardResponse response, List<string>? messages = null)
        => response.Setup(HttpStatusCode.OK, false, messages);

    public static void SetCreatedResponse(this StandardResponse response, List<string>? messages = null)
        => response.Setup(HttpStatusCode.Created, false, messages);

    public static void SetNotFoundResponse(this StandardResponse response, List<string> messages)
        => response.Setup(HttpStatusCode.NotFound, false, messages);

    public static void SetNotFoundResponse(this StandardResponse response)
        => response.Setup(HttpStatusCode.NotFound, false);

    public static void SetBadRequestResponse(this StandardResponse response, List<string> messages)
        => response.Setup(HttpStatusCode.BadRequest, true, messages);

    public static void SetInternalServerErrorResponse(this StandardResponse response, List<string> messages)
        => response.Setup(HttpStatusCode.InternalServerError, true, messages);

    public static void SetInternalServerErrorResponse(this StandardResponse response)
        => response.Setup(HttpStatusCode.InternalServerError, true);

    #endregion StandarResponse

    #region StandarResponseWithPayload

    public static void SetSuccessResponse<T>(this StandardResponse<T> response, T? payload = default, List<string>? messages = null)
        => response.Setup(HttpStatusCode.OK, false, messages, payload);

    public static void SetCreatedResponse<T>(this StandardResponse<T> response, T? payload = default, List<string>? messages = null)
        => response.Setup(HttpStatusCode.Created, false, messages, payload);

    public static void SetNotFoundResponse<T>(this StandardResponse<T> response, List<string> messages, T? payload = default)
        => response.Setup(HttpStatusCode.NotFound, false, messages, payload);

    public static void SetBadRequestResponse<T>(this StandardResponse<T> response, List<string> messages, T? payload = default)
        => response.Setup(HttpStatusCode.BadRequest, true, messages, payload);

    public static void SetInternalServerErrorResponse<T>(this StandardResponse<T> response, List<string> messages, T? payload = default)
        => response.Setup(HttpStatusCode.InternalServerError, true, messages, payload);

    public static void SetNoContentResponse<T>(this StandardResponse<T> response, T? payload = default, List<string>? messages = null)
        => response.Setup(HttpStatusCode.NoContent, false, messages, payload);

    public static void SetUnauthorizedResponse<T>(this StandardResponse<T> response, List<string> messages, T? payload = default)
        => response.Setup(HttpStatusCode.Unauthorized, true, messages, payload);

    public static void SetForbiddenResponse<T>(this StandardResponse<T> response, List<string> messages, T? payload = default)
        => response.Setup(HttpStatusCode.Forbidden, true, messages, payload);

    #endregion StandarResponseWithPayload

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StandardResponse<object> CreateValidationErrorResponse(IEnumerable<ValidationFailure> errors)
    {
        var response = new StandardResponse<object>();
        response.SetBadRequestResponse([.. errors.Select(e => e.ErrorMessage)]);
        return response;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static StandardResponse<object> CreateInternalErrorResponse()
    {
        return new StandardResponse<object>
        {
            Messages = ["An error occurred while processing the request"],
            Payload = null
        };
    }
}
