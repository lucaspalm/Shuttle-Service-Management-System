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
    [Authorize(Roles = "Manager, Administrator")]
    public class ManagersController : Controller
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();
        private SSMS_Helper ssms = new SSMS_Helper();

        //
        // GET: /Managers/Home
        public ActionResult Home()
        {
            return View();
        }

        //
        // GET: /Managers/ScheduleTrips
        public ActionResult ScheduleTrips()
        {
            return View();
        }

        //
        // GET: /Managers/ManageUserAccounts
        public ActionResult ManageUserAccounts()
        {
            // Variable Declarations
            List<ManageUserAccountsViewModel> model = new List<ManageUserAccountsViewModel>();

            // Populate the view model
            model = ssms.GetUserAccountInfo();

            return View(model);
        }

        //
        // GET: /Managers/EditUserRole
        public ActionResult EditUserRole(string userID)
        {
            // Variable Declarations
            EditUserRoleViewModel model = new EditUserRoleViewModel();

            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                // Create a selectlist of destinations to be passed to the view
                ViewBag.USER_ROLE = new SelectList(db.SYSTEM_ROLES, "Id", "Name", ssms.GetUserRole(userID.ToString()));

                ViewBag.UserName = ssms.GetUserName(userID.ToString());

                // Populate the view model
                model.userID = userID.ToString();
                model.userRoleID = ssms.GetUserRole(userID.ToString());
            }

            return View(model);
        }

        //
        // GET: /Managers/ViewAccountActivity
        public ActionResult ViewAccountActivity(string userID)
        {
            // Variable Declarations
            List<AccountActivityViewModel> model = new List<AccountActivityViewModel>();
            List<DateTime> accountActivityList = new List<DateTime>();

            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.UserName = ssms.GetUserName(userID);

                accountActivityList = ssms.GetUserLogInHistory(userID);

                foreach (DateTime item in accountActivityList)
                {
                    // Create a new object
                    AccountActivityViewModel newObject = new AccountActivityViewModel();

                    newObject.loginDateTime = item.ToString();

                    model.Add(newObject);
                }
            }

            return View(model);
        }

        //
        // GET: /Managers/ViewDriverList
        public ActionResult ViewDriverList()
        {
            // Variable Declarations
            List<DriverListViewModel> model = new List<DriverListViewModel>();

            // Populate the view model
            model = ssms.GetDriverList();

            return View(model);
        }

        //
        // GET: /Managers/ViewDriverTimesheet
        public ActionResult ViewDriverTimesheet(string userID)
        {
            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.UserID = userID;
                ViewBag.UserName = ssms.GetUserName(userID);
            }
            
            return View();
        }

        //
        // GET: /Managers/ViewDriverSchedule
        public ActionResult ViewDriverSchedule(string userID)
        {
            if (userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.UserID = userID;
                ViewBag.UserName = ssms.GetUserName(userID);
            }

            return View();
        }

        // POST: /Managers/GetDriverTimesheet
        public JsonResult GetDriverTimesheet(string start, string end, string driverUserID)
        {
            // Variable Declarations
            List<CalendarEventObject> eventListToSend = new List<CalendarEventObject>();

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

        // GET: /Managers/GetDriverSchedule
        public JsonResult GetDriverSchedule(string start, string end, string driverUserID)
        {
            // Variable Declarations
            List<CalendarEventObject> eventListToSend = new List<CalendarEventObject>();

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

        // POST: /Managers/EditUserRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRole(EditUserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update the user's role
                ssms.UpdateExistingUserRole(model.userID, model.userRoleID);

                return RedirectToAction("ManageUserAccounts");
            }

            return View(model);
        }

        // GET: /Managers/GetTripOrders
        public JsonResult GetTripOrders(string start, string end)
        {
            // Variable Declarations
            List<CalendarEventObject> eventListToSend = new List<CalendarEventObject>();

            // Get the list of all trips
            var orderList = ssms.GetAllTripOrders(start, end);

            // Convert the event list into a format that can be correctly read by the calendar plugin
            foreach (ORDER order in orderList)
            {
                // Create a new calendar event object
                CalendarEventObject newObject = new CalendarEventObject();
                bool assigned = false;

                // Assign the corresponding order values to the event object
                newObject.id = order.ORDER_NUMBER;
                newObject.title = "Order #: " + order.ORDER_NUMBER;
                newObject.start = order.DEPARTURE_DATETIME.ToString();
                newObject.end = "";

                // Check if the order has already been assigned a driver
                if (ssms.GetAssignedDriverForOrder(order.ORDER_NUMBER.ToString()) != "" && ssms.GetAssignedDriverForOrder(order.ORDER_NUMBER.ToString()) != null)
                {
                    assigned = true;
                }

                // Assign a color to the order event based on whether or not the order has already ocurred
                if (DateTime.Now > order.DEPARTURE_DATETIME)
                {
                    newObject.color = "gray";

                    if (assigned)
                    {
                        newObject.title += "\nDriver: " + ssms.GetUserName(ssms.GetAssignedDriverForOrder(order.ORDER_NUMBER.ToString()));
                    }
                    else
                    {
                        newObject.title += "\nDriver: Unassigned";
                    }
                }
                else
                {
                    if (assigned)
                    {
                        newObject.title += "\nDriver: " + ssms.GetUserName(ssms.GetAssignedDriverForOrder(order.ORDER_NUMBER.ToString()));
                        newObject.color = "green";
                    }
                    else
                    {
                        newObject.title += "\nDriver: Unassigned";
                        newObject.color = "red";
                    }
                }

                // Add the new event object into the events list to send to the calendar plugin
                eventListToSend.Add(newObject);
            }

            // Convert the event list to an array
            var rows = eventListToSend.ToArray();

            // Return the JSON list data to the calendar
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        // GET: /Managers/GetAllAvailableOrderDrivers
        public JsonResult GetAllAvailableOrderDrivers(string orderNumber)
        {
            // Variable Declarations
            List<DriverDDLObject> driverListToSend = new List<DriverDDLObject>();
            int i;

            // Get the list of all trips
            var driverList = ssms.GetAllAvailableDriversForOrder(orderNumber);

            // Convert the event list into a format that can be correctly read by the calendar plugin
            foreach (string driver in driverList)
            {
                // Create a new driver drop down list object
                DriverDDLObject newObject = new DriverDDLObject();
                newObject.value = driver;
                newObject.text = ssms.GetUserName(driver);

                // Add the new event object into the events list to send to the calendar plugin
                driverListToSend.Add(newObject);
            }

            // Check if there were no available drivers found
            if (driverListToSend.Count <= 0)
            {
                // Create a new driver drop down list object
                DriverDDLObject newObject = new DriverDDLObject();
                newObject.value = "No Available Drivers Found";
                newObject.text = "No Available Drivers Found";
                driverListToSend.Add(newObject);
            }
            else
            {
                // Check if the order has been assigned a driver yet
                if (ssms.GetAssignedDriverForOrder(orderNumber) != "" && ssms.GetAssignedDriverForOrder(orderNumber) != null)
                {
                    // Loop through the list of drivers
                    for (i = 0; i < driverListToSend.Count; i++)
                    {
                        // Look for the assigned driver in the list and move it to index 0
                        if (driverListToSend[i].value == ssms.GetAssignedDriverForOrder(orderNumber))
                        {
                            // Create a new driver drop down list object
                            DriverDDLObject newObject = new DriverDDLObject();
                            newObject.value = driverListToSend[i].value;
                            newObject.text = driverListToSend[i].text;
                            driverListToSend.Insert(0, newObject);
                            driverListToSend.RemoveAt(i);
                            break;
                        }
                    }
                }
                else
                {
                    // Create a new driver drop down list object
                    DriverDDLObject newObject = new DriverDDLObject();
                    newObject.value = "Unassigned";
                    newObject.text = "Unassigned";
                    driverListToSend.Insert(0, newObject);
                }
            }

            // Convert the event list to an array
            var rows = driverListToSend.ToArray();

            // Return the JSON driver list data to the calendar plugin
            return Json(rows, JsonRequestBehavior.AllowGet);
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

        // POST: /Managers/SaveDriverAssignment
        [HttpPost]
        public bool SaveDriverAssignment(string OrderNumber, string DriverID)
        {
            try
            {
                // Check if the order already has been assigned a driver
                if (ssms.GetAssignedDriverForOrder(OrderNumber) != null && ssms.GetAssignedDriverForOrder(OrderNumber) != "")
                {
                    // Update the existing assignment record in the database
                    ssms.UpdateExistingDriverAssignment(OrderNumber, DriverID);

                    // Send alerts to the new driver, alerting them of their new assignment
                    ssms.SendDriverAssignmentAlerts(DriverID);
                }
                else
                {
                    // Insert a new assignment record into the database
                    ssms.InsertNewDriverAssignment(OrderNumber, DriverID);

                    // Send alerts to the new driver, alerting them of their new assignment
                    ssms.SendDriverAssignmentAlerts(DriverID);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}