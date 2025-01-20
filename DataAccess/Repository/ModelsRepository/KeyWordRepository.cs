using Banha_UniverCity.Data;
using Banha_UniverCity.Models;
using BFCAI.Models;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.ModelsRepository
{
    public class KeyWordRepository : GenralRepository<KeyWord>, IKeyWordRepository
    {
        public KeyWordRepository(ApplicationDbContext context) : base(context)
        {
        }
        public void AddCourseKeyWordsInKeyWordTable(List<KeyWord> words, int? courseId, int? postId)
        {

            List<KeyWord> keyWords = new();
            if (courseId != null)
            {

                foreach (var word in words)
                {
                    keyWords.Add(new()
                    {
                        CourseId = courseId,
                        Name = word.Name,
                    });
                }
            }
            if (postId != null)
            {
                foreach (var word in words)
                {

                    keyWords.Add(new()
                    {
                        PostId = postId,
                        Name = word.Name,
                    });

                }
            }
                AddRange(keyWords);
        }
    }
}
