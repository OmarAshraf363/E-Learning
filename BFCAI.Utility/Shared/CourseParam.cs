using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Utility.Shared
{
    public class CourseParam
    {
        public string InstructorId { get; set; }=string.Empty;
        public string CourseId { get; set; }=string.Empty;
        public decimal Price { get; set; } = 0.0m;
        public string Search { get; set; } = string.Empty;
        public int CategoryId { get; set; } 
        = 0;
        public int Rate { get; set; }

        public string SortBy { get; set; } = "relevance"; // Default sort by relevance
        public string SortOrder { get; set; } = "asc"; // Default sort order ascending


    }
}
