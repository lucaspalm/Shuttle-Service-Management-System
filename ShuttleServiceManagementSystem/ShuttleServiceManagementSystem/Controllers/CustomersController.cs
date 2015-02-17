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
    [Authorize]
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
            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_CITY");
            ViewBag.USER_ID = new SelectList(db.USER_ACCOUNTS, "USER_ID", "UserName");
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

                // Insert the new order info into the orders table
                //ssms.InsertNewOrderInfo(userID, orderNumber, orderDate, model.DepartureDateTime, model.DepartureAddress,
                                        //model.DepartureCity, model.DepartureState, model.DepartureZipCode, model.Destination,
                                        //model.NumberOfPassengers, model.FlightDetails, model.Comments);

                return RedirectToAction("Home");
            }
            else

            return View(model);

            //ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_CITY", order.DESTINATION_NAME);
            //ViewBag.USER_ID = new SelectList(db.USER_ACCOUNTS, "USER_ID", "UserName", order.USER_ID);
            //return View(order);
        }
	}
}