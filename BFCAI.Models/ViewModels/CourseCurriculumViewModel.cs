using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CourseCurriculumViewModel
    {
        public string CurriculumName { get; set; }=string.Empty;
        public List<string> Resources { get; set; } = new();
        public List<string> Videos { get; set; } = new();
        public List<string> Assignments { get; set; } = new();
    }
}
