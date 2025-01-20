using Banha_UniverCity.Data;
using BFCAI.Models;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository
{
    internal class ExamRepository : GenralRepository<Exam>, IExamRepository
    {
        public ExamRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
