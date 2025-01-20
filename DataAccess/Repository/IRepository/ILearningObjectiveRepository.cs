using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface ILearningObjectiveRepository:IGenralRepository<LearningObjective>
    {
        void AddCourseOpjectivesInOpjectiveTable(List<LearningObjective> objectives, int courseId);
    }
}
