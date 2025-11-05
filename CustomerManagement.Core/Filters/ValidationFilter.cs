using CustomerManagement.Core.Requests.Utils;
using CustomerManagement.Core.Requests.Utils.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Core.Filters;

public class ValidationFilter<TRequest> : ActionFilterAttribute where TRequest : IRequest
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<TRequest>>()
            ?? throw new InvalidOperationException("Validator not found");

        TRequest? model = (TRequest?)context.ActionArguments.SingleOrDefault(x => x.Value is TRequest).Value
            ?? throw new InvalidOperationException("Model not found");

        if (model is not UserValidatedRequest)
        {
            var result = validator.ValidateAsync(model).Result;

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ValidationException("Validation error", result.Errors);
            }

            return;
        }

        //var claims = context.HttpContext.User.Claims;
        //var roleClaim = System.Security.Claims.ClaimTypes.Role;
        //var nameClaim = System.Security.Claims.ClaimTypes.Name;
        //var emailClaim = System.Security.Claims.ClaimTypes.Email;

        UserValidatedRequest? userValidatedModel =
            (UserValidatedRequest?)context.ActionArguments.SingleOrDefault(x => x.Value is TRequest).Value
            ?? throw new InvalidOperationException("Model not found");

        //userValidatedModel.UserEmail = claims.SingleOrDefault(c => c.Type == emailClaim)?.Value ?? string.Empty;
        //userValidatedModel.UserName = claims.SingleOrDefault(c => c.Type == nameClaim)?.Value ?? string.Empty;
        //userValidatedModel.UserRole = claims.SingleOrDefault(c => c.Type == roleClaim)?.Value ?? string.Empty;

        var modelToValidate = (TRequest)Convert.ChangeType(userValidatedModel, typeof(TRequest));
        var userValidatedResult = validator.ValidateAsync(modelToValidate).Result;

        if (!userValidatedResult.IsValid)
        {
            //if (userValidatedResult.Errors.Count != 0 && userValidatedResult.Errors[0].ErrorCode == HttpStatusCode.Unauthorized.ToString())
            //{
            //    throw new ForbiddenException(userValidatedResult.Errors[0].ErrorMessage);
            //}

            var errorMessages = userValidatedResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException("Validation error", userValidatedResult.Errors);
        }
    }
}
