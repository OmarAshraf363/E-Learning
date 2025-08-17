using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _env;

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<(bool , string )> UploadImageAsync(IFormFile image, string folderPath)
    {
        if (image == null || image.Length == 0)
            return (false, "Invalid image file.");

        var extension = Path.GetExtension(image.FileName);
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        if (!Array.Exists(allowedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase)))
            return (false, "Unsupported image format.");

        try
        {
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var savePath = Path.Combine(_env.WebRootPath, folderPath);

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var fullPath = Path.Combine(savePath, uniqueFileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return (true, uniqueFileName);
        }
        catch (Exception ex)
        {
            // You may log the error here
            return (false, $"Error while uploading image: {ex.Message}");
        }
    }

    public async Task<bool> DeleteImageAsync(string imagePath)
    {
        try
        {
            if (string.IsNullOrEmpty(imagePath))
                return false;

            var fullPath = Path.Combine(_env.WebRootPath, imagePath);
            if (!File.Exists(fullPath))
                return false;

            await Task.Run(() => File.Delete(fullPath));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<string> GetImageUrlAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return Task.FromResult<string>(null);

        var relativePath = $"/{imagePath.Replace("\\", "/")}";
        return Task.FromResult(relativePath);
    }
}
