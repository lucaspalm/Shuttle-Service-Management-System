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
        public void GetDriverTimesheet(double start, double end)
        {
            // Variable Declarations
            //var timesheetEvents = ssms.GetDriverTimesheet(User.Identity.GetUserId());




            //var rows = timesheetEvents.ToArray();

            //return Json(rows, JsonRequestBehavior.AllowGet);
        }

        // GET: /Drivers/SaveNewAvailability
        [HttpPost]
        public bool SaveNewAvailability(string NewAvailabilityDate, string NewAvailabilityStartTime, string NewAvailabilityEndTime)
        {
            // Variable Declarations
            string a = NewAvailabilityDate;
            string b = NewAvailabilityStartTime;
            string c = NewAvailabilityEndTime;

            string answer = a + b + c;

            return true;
        }
    }
}