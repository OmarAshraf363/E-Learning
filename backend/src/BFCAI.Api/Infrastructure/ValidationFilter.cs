using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BFCAI.Api.Infrastructure;

/// <summary>
/// Runs the FluentValidation validator (if one is registered) for every action argument.
/// Invalid input throws ValidationException -> 400 ProblemDetails (see GlobalExceptionHandler).
/// You never call validators manually: just add an AbstractValidator&lt;TRequest&gt; class.
/// </summary>
public class ValidationFilter(IServiceProvider services) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
            if (services.GetService(validatorType) is not IValidator validator) continue;

            var result = await validator.ValidateAsync(new ValidationContext<object>(argument));
            if (!result.IsValid) throw new ValidationException(result.Errors);
        }

        await next();
    }
}
