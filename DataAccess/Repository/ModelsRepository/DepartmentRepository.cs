using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;
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
        public Department getDepartmentCourses(int id) 
        {
            var department = context.Departments.Where(e => e.DepartmentID == id)
                .Include(e => e.Courses).ThenInclude(e => e.Keywords).SingleOrDefault();
            return department;
        }

        public async Task<IReadOnlyList<Department>> GetTopDepartments()
        {
            var topDepartments=await context.Departments.SelectMany(e=>e.Courses).OrderByDescending(e=>e.Enrollments.Count).Take(3)
                .Select(e=>e.Department).Distinct().ToListAsync();
            return topDepartments;
        }
    }
}
