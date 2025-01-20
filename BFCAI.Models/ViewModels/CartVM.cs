using Banha_UniverCity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class CartVM
    {
        public List<CartItems> CartItems { get; set; }= new List<CartItems>();
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}
