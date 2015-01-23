using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShuttleServiceManagementSystem.Models;
using ShuttleServiceManagementSystem.Utilities;


namespace ShuttleServiceManagementSystem.Controllers
{
  
    public class LoginController : Controller
    {
        public Database_Helper db = new Database_Helper();

        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            return View();
        }
	}
}