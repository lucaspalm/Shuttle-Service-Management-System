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

        // GET: /Drivers/GetDriverTimesheet
        public JsonResult GetTrips(double? start, double? end)
        {
            // Get the list of all trips
            var tripList = ssms.GetAllTrips();

            // Convert the event list into a format that can be correctly read by the calendar plugin
            foreach (DRIVER_TRIP_ASSIGNMENTS assign in tripList)
            {

            }

            //var eventListToSend = from e in tripList
            //                      select new
            //                      {
            //                          id = e.ID,
            //                          title = "",
            //                          start = e.DATE.Add(e.START_TIME).ToString("s"),
            //                          color = "#008CBA"
            //                      };
            //var rows = eventListToSend.ToArray();

            // Return the JSON list data to the calendar
            return null;// Json(rows, JsonRequestBehavior.AllowGet);
        }
	}
}