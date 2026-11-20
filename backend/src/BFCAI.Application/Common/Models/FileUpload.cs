namespace BFCAI.Application.Common.Models;

/// <summary>An uploaded file, independent of ASP.NET's IFormFile.</summary>
public record FileUpload(Stream Content, string FileName, long Length, string? ContentType = null)
{
    public string Extension => Path.GetExtension(FileName).ToLowerInvariant();
}

/// <summary>Allowed extensions / sizes per upload kind (same rules as the legacy services).</summary>
public static class FileRules
{
    public static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    public static readonly string[] AssignmentExtensions = [".pdf", ".docx", ".txt", ".png", ".jpg"];
    public static readonly string[] VideoExtensions = [".mp4", ".webm", ".ogg", ".mov", ".mkv", ".avi"];
    public static readonly string[] AttachmentExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp", ".pdf"];
    public const long AssignmentMaxBytes = 10 * 1024 * 1024;

    public static bool HasAllowedExtension(FileUpload file, string[] allowed) => allowed.Contains(file.Extension);
}
