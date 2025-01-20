using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class DepartmentRepository : GenralRepository<Department>, IDepartmentRepository
    {
        public ApplicationDbContext context;
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public Department getSpacifcDetails(int id)
        {
            var department = context.Departments.Where(e => e.DepartmentID == id).Include(e => e.Courses)
                 .Include(e => e.Users).Include(e => e.Events).Include(e => e.ClassSchedules).ThenInclude(e=>e.Instructor)
                 .Include(e => e.ClassSchedules).ThenInclude(e=>e.Room).FirstOrDefault();
            return department ;
        }
    }
}
