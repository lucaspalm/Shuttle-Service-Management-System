using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShuttleServiceManagementSystem.Models
{
    public class ManageUserAccountsViewModel
    {
        public string userID { get; set; }

        [Display(Name = "Username")]
        public string userName { get; set; }

        [Display(Name = "Role")]
        public string userRole { get; set; }
    }

    public class EditUserRoleViewModel
    {
        public string userID { get; set; }

        [Display(Name = "User Role")]
        public string userRoleID { get; set; }
    }

    public class AccountActivityViewModel
    {
        [Display(Name = "Log-In Date/Time")]
        public string loginDateTime { get; set; }
    }

    public class DriverListViewModel
    {
        public string userID { get; set; }

        [Display(Name = "Driver Username")]
        public string userName { get; set; }
    }

    public class DriverHourlyRateViewModel
    {
        [Display(Name = "Hourly Rate")]
        public double hourlylRate { get; set; }
    }
}