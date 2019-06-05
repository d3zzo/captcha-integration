using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace captcha_integration.Models
{
    public class UserViewModel
    {
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public string  LastName { get; set; }
        public bool displayCaptcha { get; set; }
    }
}
