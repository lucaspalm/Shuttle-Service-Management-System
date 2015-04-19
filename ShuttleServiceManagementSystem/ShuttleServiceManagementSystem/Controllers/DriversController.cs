using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ShuttleServiceManagementSystem.Models;
using ShuttleServiceManagementSystem.Utilities;
using System.Collections.Generic;
using SSMSDataModel.DAL;
using System.Net;
using System;

namespace ShuttleServiceManagementSystem.Controllers
{
    [Authorize(Roles = "Driver, Administrator")]
    public class DriversController : Controller
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();
        private SSMS_Helper ssms = new SSMS_Helper();

        // GET: /Drivers/Home
        public ActionResult Home()
        {
            return View();
        }

        // GET: /Drivers/ManageTimesheet
        public ActionResult ManageTimesheet()
        {
            return View();
        }

        // GET: /Drivers/ViewSchedule
        public ActionResult ViewSchedule()
        {
            return View();
        }

        // GET: /Drivers/ViewSchedule
        public ActionResult ViewDrivingHistory()
        {
            // Variable Declarations
            string userID = "";
            string destination_name = "";
            List<ORDER> databaseOrderList = new List<ORDER>();
            List<ViewOrdersViewModel> viewModelOrderList = new List<ViewOrdersViewModel>();

            // Get the current user's id
            userID = User.Identity.GetUserId();

            // Get the list of user orders from the database
            databaseOrderList = ssms.GetDriverTripHistory(userID);

            // Populate the view model
            foreach (ORDER item in databaseOrderList)
            {
                // Create a temp ViewOrdersViewModel object
                ViewOrdersViewModel tempModelObject = new ViewOrdersViewModel();

                // Fill the temp ViewOrdersViewModel object up with data               
                tempModelObject.OrderNumber = item.ORDER_NUMBER;
                tempModelObject.OrderDate = item.DATETIME_ORDER_PLACED;
                tempModelObject.DepartureDate = item.DEPARTURE_DATETIME;
                destination_name = ssms.GetDestinationName(item.DESTINATION_ID.ToString());
                tempModelObject.DestinationName = destination_name;

                // Add the temp object to the view model list
                viewModelOrderList.Add(tempModelObject);
            }

            return View(viewModelOrderList);
        }

        // GET: /Drivers/OrderDetails
        public ActionResult OrderDetails(int? id)
        {
            // Variable Declarations
            OrderDetailsViewModel model = new OrderDetailsViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ORDER order = db.ORDERS.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Populate the view model
                model.OrderNumber = order.ORDER_NUMBER;
                model.OrderDate = order.DATETIME_ORDER_PLACED;
                model.DepartureDate = order.DEPARTURE_DATETIME;
                model.DepartureAddress = order.DEPARTURE_STREET_ADDRESS;
                model.DepartureCity = order.DEPARTURE_CITY;
                model.DepartureState = order.DEPARTURE_STATE;
                model.DepartureZipCode = order.DEPARTURE_ZIPCODE;
                model.DestinationName = ssms.GetDestinationName(order.DESTINATION_ID.ToString());
                model.NumberOfPassengers = order.NUMBER_OF_PASSENGERS;
                model.FlightDetails = order.FLIGHT_DETAILS;
                model.Comments = order.COMMENTS;
            }

            // Determine if this order is a past order or not
            if (order.DEPARTURE_DATETIME > DateTime.Now)
            {
                ViewBag.PastOrder = false;
            }
            else
            {
                ViewBag.PastOrder = true;
            }

            return View(model);
        }

        // GET: /Drivers/GetDriverTimesheet
        public JsonResult GetDriverOrders(string start, string end)
        {
            // Variable Declarations
            string driverUserID = "";
            List<CalendarEventObject> eventListToSend = new List<CalendarEventObject>();

            // Get the current user's ID
            driverUserID = User.Identity.GetUserId();

            // Get the list of timesheet events for the current user/driver
            var driverOrders = ssms.GetDriverOrders(driverUserID, start, end);

            // Convert the event list into a format that can be correctly read by the calendar plugin
            foreach (ORDER order in driverOrders)
            {
                // Create a new calendar event object
                CalendarEventObject newObject = new CalendarEventObject();

                // Assign the corresponding order values to the event object
                newObject.id = order.ORDER_NUMBER;
                newObject.title = "Order #: " + order.ORDER_NUMBER;
                newObject.start = order.DEPARTURE_DATETIME.ToString();
                newObject.end = "";

                // Assign a color to the order event based on whether or not the order has already ocurred
                if (DateTime.Now > order.DEPARTURE_DATETIME)
                {
                    newObject.color = "gray";                  
                }
                else
                {
                    newObject.color = "#008CBA";
                }
                
                // Add the new event object into the events list to send to the calendar plugin
                eventListToSend.Add(newObject);
            }

            // Convert the event list to an array
            var rows = eventListToSend.ToArray();

            // Return the JSON list data to the calendar
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        // GET: /Drivers/GetDriverTimesheet
        public JsonResult GetDriverTimesheet(string start, string end)
        {
            // Variable Declarations
            string driverUserID = "";
            List<CalendarEventObject> eventListToSend = new List<CalendarEventObject>();

            // Get the current user's ID
            driverUserID = User.Identity.GetUserId();

            // Get the list of timesheet events for the current user/driver
            var timesheetEvents = ssms.GetDriverTimesheet(driverUserID, start, end);

            // Convert the event list into a format that can be correctly read by the calendar plugin
            foreach (DRIVER_TIME_AVAILABILITIES avails in timesheetEvents)
            {
                // Create a new calendar event object
                CalendarEventObject newObject = new CalendarEventObject();

                // Assign the corresponding order values to the event object
                newObject.id = avails.ID;
                newObject.title = "";
                newObject.start = avails.DATE.Add(avails.START_TIME).ToString("s");
                newObject.end = avails.DATE.Add(avails.END_TIME).ToString("s");
                newObject.color = "#008CBA";

                // Add the new event object into the events list to send to the calendar plugin
                eventListToSend.Add(newObject);
            }

            // Convert the event list to an array
            var rows = eventListToSend.ToArray();

            // Return the JSON list data to the calendar
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        // POST: /Drivers/SaveNewAvailability
        [HttpPost]
        public bool SaveNewAvailability(string NewAvailabilityDate, string NewAvailabilityStartTime, string NewAvailabilityEndTime)
        {
            // Variable Declarations
            string availabilityID = "";
            string driverUserID = "";

            try
            {
                // Get the next available ID to be used for the new driver availability
                availabilityID = ssms.GetNextAvailabilityID().ToString();

                // Get the current user's ID
                driverUserID = User.Identity.GetUserId();

                // Insert the new driver availability into the database
                ssms.InsertNewAvailability(availabilityID, driverUserID, NewAvailabilityDate, NewAvailabilityStartTime, NewAvailabilityEndTime);
            }
            catch
            {
                return false;
            }

            return true;
        }

        // POST: /Drivers/SaveNewAvailability
        [HttpPost]
        public bool DeleteAvailability(string AvailabilityID)
        {
            try
            {
                // Insert the new driver availability into the database
                ssms.DeleteAvailability(AvailabilityID);
            }
            catch
            {
                return false;
            }

            return true;
        }

        // GET: /Managers/GetAllOrderInfo
        public JsonResult GetAllOrderInfo(string orderNumber)
        {
            // Variable Declarations
            List<OrderInfoObject> orderInfo = new List<OrderInfoObject>();
            ORDER order = new ORDER();
            OrderInfoObject newObject = new OrderInfoObject();

            // Query the order information
            order = db.ORDERS.Find(Convert.ToInt32(orderNumber));

            // Populate the OrderInfo object to send back as a JSON structure
            newObject.customerName = ssms.GetUserFirstandLastNameFromOrder(orderNumber);
            newObject.dateOrderPlaced = order.DATETIME_ORDER_PLACED.ToString();
            newObject.departureDate = order.DEPARTURE_DATETIME.ToString();
            newObject.departureAddress = order.DEPARTURE_STREET_ADDRESS.ToString();
            newObject.departureCity = order.DEPARTURE_CITY.ToString();
            newObject.departureState = order.DEPARTURE_STATE.ToString();
            newObject.departureZipCode = order.DEPARTURE_ZIPCODE.ToString();
            newObject.destination = ssms.GetDestinationName(order.DESTINATION_ID.ToString());
            newObject.numPassengers = order.NUMBER_OF_PASSENGERS.ToString();

            if (order.FLIGHT_DETAILS != null && order.FLIGHT_DETAILS.ToString() != "")
            {
                newObject.flightDetails = order.FLIGHT_DETAILS.ToString();
            }
            else
            {
                newObject.flightDetails = "None";
            }

            if (order.COMMENTS != null && order.COMMENTS.ToString() != "")
            {
                newObject.comments = order.COMMENTS.ToString();
            }
            else
            {
                newObject.comments = "None";
            }

            orderInfo.Add(newObject);

            var rows = orderInfo.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }
    }
}