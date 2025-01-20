using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;

namespace Banha_UniverCity.Repository.ModelsRepository
{
    public class FeedbackRepository : GenralRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
