using DataAccess.Repository.IRepository;
using DataAccess.Repository.IRepository.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository.Service
{
    public class AssinmentService : IAssinmentService
    {
        

        public async Task<string> UploadAssignmentAsync(IFormFile file)
        {
            if (!ValidateFile(file))
                return null;

            // توليد اسم فريد للملف
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            // تحديد المسار الفعلي للحفظ
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assignments");

            // إنشاء المجلد إذا لم يكن موجودًا
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // حفظ الملف
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // هنا يمكنك حفظ مسار الملف في قاعدة البيانات إذا أردت

            return  $"/assignments/{uniqueFileName}";
            

        }


        private bool ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".pdf", ".docx", ".txt", ".png", ".jpg" };
            var maxFileSize = 10 * 1024 * 1024; // 10MB

            var extension = Path.GetExtension(file.FileName).ToLower();

            return allowedExtensions.Contains(extension) && file.Length <= maxFileSize;
        }

        public bool DeleteAssignmentFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            // المسار الكامل للملف في السيرفر
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "assignments", fileName);

            if (!System.IO.File.Exists(filePath))
                return false;

            try
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }




    }
}
