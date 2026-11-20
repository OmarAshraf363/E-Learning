using BFCAI.Application.Common.Models;

namespace BFCAI.Application.Common.Interfaces;

/// <summary>Saves / deletes uploaded files (local disk today, can be swapped for blob storage).</summary>
public interface IFileStorage
{
    /// <summary>Saves the file under <paramref name="folder"/> with the given name and returns the name.</summary>
    Task<string> SaveAsync(FileUpload file, string folder, string fileName);

    /// <summary>Deletes a file by its path relative to the storage root. Missing files are ignored.</summary>
    Task DeleteAsync(string? relativePath);

    /// <summary>Opens a file for download, or null when it does not exist.</summary>
    Stream? OpenRead(string relativePath);
}
