using BFCAI.Application.Common.Interfaces;
using BFCAI.Application.Common.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BFCAI.Infrastructure.Files;

/// <summary>Stores files on the local disk under <see cref="StorageOptions.RootPath"/>.</summary>
public class LocalFileStorage : IFileStorage
{
    private readonly string _root;

    public LocalFileStorage(IOptions<StorageOptions> options, IHostEnvironment env)
    {
        var configured = options.Value.RootPath;
        _root = Path.GetFullPath(Path.IsPathRooted(configured) ? configured : Path.Combine(env.ContentRootPath, configured));
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(FileUpload file, string folder, string fileName)
    {
        var directory = ResolveSafePath(folder);
        Directory.CreateDirectory(directory);

        var safeName = Path.GetFileName(fileName);
        await using var stream = File.Create(Path.Combine(directory, safeName));
        await file.Content.CopyToAsync(stream);
        return safeName;
    }

    public Task DeleteAsync(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return Task.CompletedTask;

        var fullPath = ResolveSafePath(relativePath);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public Stream? OpenRead(string relativePath)
    {
        var fullPath = ResolveSafePath(relativePath);
        return File.Exists(fullPath) ? File.OpenRead(fullPath) : null;
    }

    /// <summary>Resolves a path inside the storage root and blocks path traversal ("../").</summary>
    private string ResolveSafePath(string relativePath)
    {
        var trimmed = relativePath.Replace('\\', '/').TrimStart('/');
        var fullPath = Path.GetFullPath(Path.Combine(_root, trimmed));
        if (!fullPath.StartsWith(_root, StringComparison.Ordinal))
            throw new InvalidOperationException("Invalid file path.");
        return fullPath;
    }
}
