namespace BFCAI.Infrastructure.Files;

public class StorageOptions
{
    public const string SectionName = "Storage";

    /// <summary>
    /// Root folder for uploaded files. Relative paths are resolved from the API content root.
    /// Defaults to the API's wwwroot so files are served at /Covers/..., /videos/..., etc.
    /// </summary>
    public string RootPath { get; set; } = "wwwroot";
}
