using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository.Service
{
    public class VedioService : IvedioService
    {
        private readonly IWebHostEnvironment _env;

        public VedioService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadVedioAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.", nameof(file));

            var uploadsFolder = Path.Combine(_env.WebRootPath, "videos");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return relative path for storage in DB or further use
            return Path.Combine("videos", uniqueFileName).Replace("\\", "/");
        }
    }
}
