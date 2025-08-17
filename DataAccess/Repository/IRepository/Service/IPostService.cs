using BFCAI.Models;
using BFCAI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository.Service
{
    public interface IPostService
    {
        Task<IReadOnlyList<PostVM>> GetRelatedPostsByKeyWordsAsync(string studentId);
    }
}
