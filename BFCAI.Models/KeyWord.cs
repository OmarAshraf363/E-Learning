using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models
{
    public class KeyWord
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter the word here .")]
        public string Name { get; set; } = string.Empty;
        //navigation property 
        public int? PostId { get; set; }
        public Post? Post { get; set; }

        public int? CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
