using BFCAI.Application.Common.Models;
using FluentValidation;
using FluentValidation.Results;

namespace BFCAI.Application.Common;

public static class Validation
{
    /// <summary>Throws a 400 validation error for one field (for rules that need data, e.g. "cover required on create").</summary>
    public static void Fail(string property, string message)
        => throw new ValidationException([new ValidationFailure(property, message)]);

    /// <summary>FluentValidation rule for an optional uploaded image.</summary>
    public static IRuleBuilderOptions<T, FileUpload?> MustBeImage<T>(this IRuleBuilder<T, FileUpload?> rule)
        => rule.Must(f => f is null || FileRules.HasAllowedExtension(f, FileRules.ImageExtensions))
            .WithMessage("Only image files are allowed (jpg, jpeg, png, gif, webp).");
}
