using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public interface IDepartmentRepository: IGenralRepository<Department>
    {
        Department getSpacifcDetails(int id);
    }
}
