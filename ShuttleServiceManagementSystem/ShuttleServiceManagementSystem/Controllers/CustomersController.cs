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
using System.Net;

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
                ssms.InsertNewOrderInfo(userID, orderNumber, orderDate, model.DepartureDate.ToString(), model.DepartureAddress,
                                        model.DepartureCity, model.DepartureState, model.DepartureZipCode, model.DestinationID.ToString(),
                                        model.NumberOfPassengers.ToString(), model.FlightDetails, model.Comments);

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

        // GET: /Customers/ViewOrders
        [HttpGet]
        public ActionResult ViewOrders()
        {
            // Variable Declarations
            string userID = "";
            string destination_name = "";
            ViewOrdersViewModel tempModelObject = new ViewOrdersViewModel();
            List<ORDER> databaseOrderList = new List<ORDER>();
            List<ViewOrdersViewModel> viewModelOrderList = new List<ViewOrdersViewModel>();

            // Get the user id
            userID = User.Identity.GetUserId();

            // Create a Current/Past/All selectlist
            //ViewBag.ORDERS = new SelectList(db.DESTINATIONS, "DESTINATION_ID", "DESTINATION_NAME");

            // Get the list of user orders from the database
            databaseOrderList = ssms.GetCurrentUserOrders(userID);

            // Populate the view model
            foreach (ORDER item in databaseOrderList)
            {
                // Fill the temp ViewOrdersViewModel object up with data
                tempModelObject.OrderNumber = item.ORDER_NUMBER;
                tempModelObject.OrderDate = item.DATETIME_ORDER_PLACED;
                tempModelObject.DepartureDate = item.DEPARTURE_DATETIME;
                destination_name = ssms.GetDestinationName(item.DESTINATION_ID.ToString());
                tempModelObject.DestinationName = destination_name;

                // Add the temp object to the view model list
                viewModelOrderList.Add(tempModelObject);
            }

            return View(viewModelOrderList);
        }

        // GET: /Customers/OrderDetails
        public ActionResult OrderDetails(int? id)
        {
            // Variable Declarations
            OrderDetailsViewModel model = new OrderDetailsViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ORDER order = db.ORDERS.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Populate the view model
                model.OrderNumber = order.ORDER_NUMBER;
                model.OrderDate = order.DATETIME_ORDER_PLACED;
                model.DepartureDate = order.DEPARTURE_DATETIME;
                model.DepartureAddress = order.DEPARTURE_STREET_ADDRESS;
                model.DepartureCity = order.DEPARTURE_CITY;
                model.DepartureState = order.DEPARTURE_STATE;
                model.DepartureZipCode = order.DEPARTURE_ZIPCODE;
                model.DestinationName = ssms.GetDestinationName(order.DESTINATION_ID.ToString());
                model.FlightDetails = order.FLIGHT_DETAILS;
                model.Comments = order.COMMENTS;
            }

            return View(model);
        }

        // GET: /Customers/EditOrder
        public ActionResult EditOrder(int? id)
        {
            // Variable Declarations
            EditOrderViewModel model = new EditOrderViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ORDER order = db.ORDERS.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Create a selectlist of destinations to be passed to the view
                ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_ID", "DESTINATION_NAME", order.DESTINATION_ID);

                // Populate the view model
                model.OrderNumber = order.ORDER_NUMBER;
                model.DepartureDate = order.DEPARTURE_DATETIME;
                model.DepartureAddress = order.DEPARTURE_STREET_ADDRESS;
                model.DepartureCity = order.DEPARTURE_CITY;
                model.DepartureState = order.DEPARTURE_STATE;
                model.DepartureZipCode = order.DEPARTURE_ZIPCODE;
                model.NumberOfPassengers = order.NUMBER_OF_PASSENGERS;
                model.FlightDetails = order.FLIGHT_DETAILS;
                model.Comments = order.COMMENTS;
            }

            return View(model);
        }

        // POST: /Customers/EditOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(EditOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Update the order info
                ssms.UpdateOrderInfo(model.OrderNumber.ToString(), model.DepartureDate.ToString(), model.DepartureAddress,
                                     model.DepartureCity, model.DepartureState, model.DepartureZipCode, model.DestinationID.ToString(), 
                                     model.NumberOfPassengers.ToString(), model.FlightDetails, model.Comments);

                return RedirectToAction("Home");
            }

            return View(model);
        }

        // GET: /Customers/DeleteOrder
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDER order = db.ORDERS.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /Customers/DeleteOrder
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ORDER order = db.ORDERS.Find(id);
            db.ORDERS.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}