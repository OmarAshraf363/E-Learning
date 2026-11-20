using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace BFCAI.Application.Common;

public static class IdentityResultExtensions
{
    /// <summary>Turns Identity errors (e.g. "Passwords must have at least one digit") into a 400 validation response.</summary>
    public static void ThrowIfFailed(this IdentityResult result, string property = "")
    {
        if (result.Succeeded) return;
        throw new ValidationException(result.Errors.Select(e => new ValidationFailure(property, e.Description)));
    }
}
