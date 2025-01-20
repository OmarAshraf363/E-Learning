using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository
{
    public class LearningObjectiveRepository : GenralRepository<LearningObjective>, ILearningObjectiveRepository
    {
        public LearningObjectiveRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void AddCourseOpjectivesInOpjectiveTable(List<LearningObjective> objectives, int courseId)
        {
            List<LearningObjective> learningObjectives = new();
            foreach (var item in objectives)
            {
                learningObjectives.Add(new()
                {
                    CourseID = courseId,
                    Objective = item.Objective,
                });
            }
            AddRange(learningObjectives);
        }
    }
}
