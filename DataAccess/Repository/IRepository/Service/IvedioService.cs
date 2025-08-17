using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface IvedioService
    {
        public Task<string> UploadVedioAsync(IFormFile src);
    }
}
