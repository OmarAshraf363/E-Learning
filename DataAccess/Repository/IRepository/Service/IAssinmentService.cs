using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface IAssinmentService
    {
        Task<string> UploadAssignmentAsync(IFormFile file);
        public bool DeleteAssignmentFile(string fileName);
    }
}
