using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace DataAccess.Repository.IRepository
{
    public interface IDepartmentRepository : IGenralRepository<Department>
    {
        Department getSpacifcDetails(int id);
        public Department getDepartmentCourses(int id);
    }
}
