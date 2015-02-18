using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShuttleServiceManagementSystem.Controllers
{
    [Authorize(Roles = "Driver, Administrator")]
    public class DriversController : Controller
    {
        //
        // GET: /Drivers/
        public ActionResult Home()
        {
            return View();
        }
	}
}