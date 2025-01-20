using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class EnrollmentRepository : GenralRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
