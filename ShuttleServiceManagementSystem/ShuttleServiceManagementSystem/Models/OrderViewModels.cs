using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ShuttleServiceManagementSystem.Utilities;
using System;

namespace ShuttleServiceManagementSystem.Models
{
    public class CreateOrderViewModel
    {
        private SSMS_Helper ssms = new SSMS_Helper();

        [Required]
        [Display(Name = "Departure Date")]
        public DateTime DepartureDateTime { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string DepartureAddress { get; set; }

        [Required]
        [Display(Name = "City")]
        public string DepartureCity { get; set; }

        [Required]
        [Display(Name = "State")]
        public string DepartureState { get; set; }

        [Required]
        [Display(Name = "Zip Code")]
        public string DepartureZipCode { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string Destination { get; set; }

        [Required]
        [Display(Name = "# of Passengers")]
        public string NumberOfPassengers { get; set; }

        [Display(Name = "Flight Details")]
        public string FlightDetails { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }
    }
}