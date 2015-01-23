using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace ShuttleServiceManagementSystem.Models
{
    public class LoginModel
    {
        [Required]
        public string userName { get; set; };

        [Required]
        public string password { get; set; };
    }
}
