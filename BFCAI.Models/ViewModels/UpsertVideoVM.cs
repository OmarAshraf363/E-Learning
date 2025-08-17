using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class UpsertVideoVM
    {
        public IFormFile VedioUrl { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int CurriculumId {  get; set; }
        public int CourseId { get; set; }

    }
}
