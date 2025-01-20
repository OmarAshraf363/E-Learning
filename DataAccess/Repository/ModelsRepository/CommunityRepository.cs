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
    public class CommunityRepository : GenralRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
