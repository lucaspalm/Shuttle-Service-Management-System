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

        // Method invoked on first page load
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // Method invoked on postbacks
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            // Variable Declarations
            string username = "";
            string password = "";

            // Ensure the model parameter was passed in correctly
            if (ModelState.IsValid)
            {
                // Collect the username and password inputs from the model
                if (model.userName != null && model.userName != "")
                {
                    username = model.userName;
                }

                if (model.password != null && model.password != "")
                {
                    password = model.password;
                }

                // Validate the username/password in the database


                // Redirect the user to the home page
                return RedirectToAction("index", "home");

            }
            
            return View();
        }
	}
}