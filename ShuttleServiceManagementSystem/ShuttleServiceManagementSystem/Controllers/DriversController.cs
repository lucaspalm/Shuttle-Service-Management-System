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
using SSMSDataModel.DAL;
using System.Net;

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

        // GET: /Drivers/Timesheet
        public ActionResult ManageTimesheet()
        {
            return View();
        }

        // GET: /Drivers/Timesheet
        public ActionResult ViewSchedule()
        {
            return View();
        }

        // GET: /Drivers/GetDriverTimesheet
        public JsonResult GetDriverTimesheet(double? start, double? end)
        {
            // Variable Declarations
            string driverUserID = "";

            // Get the current user's ID
            driverUserID = User.Identity.GetUserId();

            // Get the list of timesheet events for the current user/driver
            var timesheetEvents = ssms.GetDriverTimesheet(driverUserID);

            // Convert the event list into a format that can be correctly read by the calendar plugin
            var eventListToSend = from e in timesheetEvents
                                  select new
                                  {
                                      id = e.ID,
                                      title = "",
                                      start = e.DATE.Add(e.START_TIME).ToString("s"),
                                      end = e.DATE.Add(e.END_TIME).ToString("s"),
                                      color = "#008CBA"
                                  };
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
    }
}