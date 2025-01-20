using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using Banha_UniverCity.Repository.IRepository;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository
{
    public class TopicCoveresRepository : GenralRepository<TopicCovered>, ITopicCoveresRepository
    {
       
        public TopicCoveresRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void AddCourseTopicsInTopicTable(List<TopicCovered> topics,int courseId)
        {
            List<TopicCovered> topicCoveres = new();
            foreach (var topic in topics)
            {
                topicCoveres.Add(new()
                {
                    CourseID = courseId,
                    Topic = topic.Topic,
                });
            }
            AddRange(topicCoveres);
        }
    }
}
