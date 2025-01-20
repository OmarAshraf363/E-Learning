using Banha_UniverCity.Repository.IRepository;
using BFCAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IKeyWordRepository:IGenralRepository<KeyWord>
    {
        public void AddCourseKeyWordsInKeyWordTable(List<KeyWord> words, int? courseId, int? postId);
    }
}
