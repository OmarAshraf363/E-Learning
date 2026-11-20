namespace BFCAI.Application.Common;

/// <summary>
/// Folder names and URL helpers for uploaded files. The DB keeps the same values as the
/// legacy app (e.g. only the file name for covers), DTOs expose a ready-to-use relative URL.
/// </summary>
public static class FilePaths
{
    public const string CoversFolder = "Covers";

    /// <summary>"abc.png" -> "/Covers/abc.png" (null when there is no cover).</summary>
    public static string? Cover(string? fileName)
        => string.IsNullOrWhiteSpace(fileName) ? null : $"/{CoversFolder}/{fileName}";

    /// <summary>Legacy admin naming for uploaded covers: {guid}_{original file name}.</summary>
    public static string NewCoverName(string originalFileName)
        => $"{Guid.NewGuid()}_{Path.GetFileName(originalFileName)}";
}
