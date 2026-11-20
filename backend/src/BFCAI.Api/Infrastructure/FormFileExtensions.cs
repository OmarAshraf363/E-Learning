using BFCAI.Application.Common.Models;

namespace BFCAI.Api.Infrastructure;

public static class FormFileExtensions
{
    /// <summary>Converts ASP.NET's IFormFile into the Application layer's FileUpload.</summary>
    public static FileUpload? ToFileUpload(this IFormFile? file)
        => file is null || file.Length == 0
            ? null
            : new FileUpload(file.OpenReadStream(), file.FileName, file.Length, file.ContentType);
}
