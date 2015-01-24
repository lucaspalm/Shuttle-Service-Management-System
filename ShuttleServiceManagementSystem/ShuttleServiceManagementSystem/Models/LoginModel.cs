using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace ShuttleServiceManagementSystem.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The Username field is required!")]
        public string userName { get; set; }

        [Required(ErrorMessage = "The Password field is required!")]
        public string password { get; set; }
    }
}
