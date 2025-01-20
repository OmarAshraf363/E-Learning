using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Models.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }=string.Empty;

        [Required]
        public string UserName { get; set; }= string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; }=string.Empty ;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

    }
}
