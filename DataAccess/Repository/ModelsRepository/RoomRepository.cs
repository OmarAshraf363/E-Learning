using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using DataAccess.Repository.ModelsRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class RoomRepository : GenralRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
