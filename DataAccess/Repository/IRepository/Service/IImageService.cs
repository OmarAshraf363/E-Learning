using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface IImageService
    {
        Task<(bool,string)> UploadImageAsync(IFormFile image, string folderPath);
        Task<bool> DeleteImageAsync(string imagePath);
        Task<string> GetImageUrlAsync(string imagePath);
    }
}
