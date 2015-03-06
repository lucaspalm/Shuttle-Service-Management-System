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

        // GET: /Drivers/SaveNewAvailability
        public void AddNewAvailability()
        {

        }
	}
}