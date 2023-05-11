using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    [NotMapped]
    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}
