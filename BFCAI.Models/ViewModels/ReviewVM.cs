using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CourseReviewSectionVM
    {
        public ReviewVM ReviewVM { get; set; }=new ReviewVM();
        public List<FeedbackVM> FeedbackVM { get; set; } = new List<FeedbackVM>();
    }
    public class ReviewVM
    {
        public int TotalReviews { get; set; }
        public int TotalReviewWith5 { get; set; }
        public int TotalReviewWith4 { get; set; }

        public int TotalReviewWith3 { get; set; }

        public int TotalReviewWith2 { get; set; }

        public int TotalReviewWith1 { get; set; }
        public int FinalRate { get; set; }

    }
}
