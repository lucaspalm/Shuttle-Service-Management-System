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
        [DataType(DataType.DateTime)]
        public DateTime DepartureDate { get; set; }

        [Required]
        [Display(Name = "Departure Address")]
        public string DepartureAddress { get; set; }

        [Required]
        [Display(Name = "Departure City")]
        public string DepartureCity { get; set; }

        [Required]
        [Display(Name = "Departure State")]
        public string DepartureState { get; set; }

        [Required]
        [Display(Name = "Departure Zip Code")]
        public string DepartureZipCode { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public int DestinationID { get; set; }

        [Required]
        [Display(Name = "# of Passengers")]
        public int NumberOfPassengers { get; set; }

        [Display(Name = "Flight Details")]
        public string FlightDetails { get; set; }

        [Display(Name = "Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }

    public class ViewOrdersViewModel
    {
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> OrderDate { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> DepartureDate { get; set; }

        [Display(Name = "Destination")]
        public string DestinationName { get; set; }
    }

    public class OrderDetailsViewModel
    {
        [Key]
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> OrderDate { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> DepartureDate { get; set; }

        [Display(Name = "Departure Address")]
        public string DepartureAddress { get; set; }

        [Display(Name = "Departure City")]
        public string DepartureCity { get; set; }

        [Display(Name = "Departure State")]
        public string DepartureState { get; set; }

        [Display(Name = "Departure ZipCode")]
        public string DepartureZipCode { get; set; }

        [Display(Name = "Destination")]
        public string DestinationName { get; set; }

        [Display(Name = "Passengers")]
        public int NumberOfPassengers { get; set; }

        [Display(Name = "Flight Details")]
        public string FlightDetails { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }
    }

    public class EditOrderViewModel
    {
        [Required]
        [Display(Name = "Order Number")]
        public int OrderNumber { get; set; }

        [Required]
        [Display(Name = "Departure Date")]
        [DataType(DataType.DateTime)]
        public Nullable<DateTime> DepartureDate { get; set; }

        [Required]
        [Display(Name = "Departure Address")]
        public string DepartureAddress { get; set; }

        [Required]
        [Display(Name = "Departure City")]
        public string DepartureCity { get; set; }

        [Required]
        [Display(Name = "Departure State")]
        public string DepartureState { get; set; }

        [Required]
        [Display(Name = "Departure Zip Code")]
        public string DepartureZipCode { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public Nullable<int> DestinationID { get; set; }

        [Required]
        [Display(Name = "# of Passengers")]
        public Nullable<int> NumberOfPassengers { get; set; }

        [Display(Name = "Flight Details")]
        public string FlightDetails { get; set; }

        [Display(Name = "Comments")]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}