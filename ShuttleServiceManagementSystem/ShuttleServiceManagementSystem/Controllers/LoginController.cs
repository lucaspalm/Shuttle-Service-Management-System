using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShuttleServiceManagementSystem.Models;
using ShuttleServiceManagementSystem.Utilities;
using System.Web.Security;


namespace ShuttleServiceManagementSystem.Controllers
{
  
    public class LoginController : Controller
    {
        public SSMS_Helper ssms = new SSMS_Helper();

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
                    username = model.userName.Trim();
                }

                if (model.password != null && model.password != "")
                {
                    password = model.password.Trim();
                }

                // Validate the username/password in the database
                if (ssms.IsValidLogin(username, password))
                {
                    // Create session variables to hold user specific values
                    Session["username"] = username;
                    Session["accessLevel"] = ssms.GetUserAccessLevel(username);

                    // Set an authorization cookie for this user
                    FormsAuthentication.SetAuthCookie(username, false);

                    // Redirect the user to the home page
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password does not exist!");
                }

                

            }
            
            return View();
        }
	}
}