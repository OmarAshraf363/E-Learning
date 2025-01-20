using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class ClassScheduleRepository : GenralRepository<ClassSchedule>, IClassScheduleRepository
    {
        public ClassScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
