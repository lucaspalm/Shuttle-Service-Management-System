using System;
using System.Collections.Generic;
using System.Data;
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

namespace ShuttleServiceManagementSystem.Controllers
{
    [Authorize(Roles = "Customer, Administrator")]
    public class CustomersController : Controller
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();
        private SSMS_Helper ssms = new SSMS_Helper();

        // GET: /Customers/Home
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        // GET: /Customers/CreateOrder
        [HttpGet]
        public ActionResult CreateOrder()
        {
            // Create a selectlist of destinations to be passed to the view
            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_ID", "DESTINATION_NAME");

            return View();
        }

        // POST: /Customers/CreateOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(CreateOrderViewModel model)
        {
            // Variable Declarations
            string userID = "";
            string orderNumber = "";
            string orderDate = "";

            // Check if the state of the model is OK
            if (ModelState.IsValid)
            {
                // Get the user id
                userID = User.Identity.GetUserId();

                // Get the order number
                orderNumber = ssms.GetNextOrderNumber().ToString();

                // Get the order date
                orderDate = DateTime.Now.ToString();

                // Insert the new order info into the ORDERS table
                ssms.InsertNewOrderInfo(userID, orderNumber, orderDate, model.DepartureDateTime.ToString(), model.DepartureAddress,
                                        model.DepartureCity, model.DepartureState, model.DepartureZipCode, model.Destination,
                                        model.NumberOfPassengers, model.FlightDetails, model.Comments);

                // Send a confirmation order to the user
                ssms.SendOrderConfirmationEmail(userID);

                return RedirectToAction("Home");
            }
            else
            {
                ModelState.AddModelError("", "Opps!  Something went wrong!  Please check all your field inputs and try again.");
            }

            // Create a selectlist of destinations to be passed to the view
            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_NAME");

            return View(model);
        }
	}
}