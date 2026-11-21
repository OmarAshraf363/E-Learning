using BFCAI.Application.Common.Models;
using FluentValidation;

namespace BFCAI.Application.Features.Communities;

public class PostRequestValidator : AbstractValidator<PostRequest>
{
    public PostRequestValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Attachment!)
            .Must(f => FileRules.HasAllowedExtension(f, FileRules.ImageExtensions))
            .When(x => x.Attachment is not null)
            .WithMessage("Only image files are allowed.");
    }
}

public class CommentRequestValidator : AbstractValidator<CommentRequest>
{
    public CommentRequestValidator() => RuleFor(x => x.Content).NotEmpty().WithMessage("Reply content cannot be empty.");
}
