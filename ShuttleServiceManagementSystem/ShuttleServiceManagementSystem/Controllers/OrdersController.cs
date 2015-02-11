using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSMSDataModel.DAL;

namespace ShuttleServiceManagementSystem.Controllers
{
    public class OrdersController : Controller
    {
        private SDSU_SchoolEntities db = new SDSU_SchoolEntities();

        // GET: /Orders/
        public ActionResult Index()
        {
            var orders = db.ORDERS.Include(o => o.DESTINATION).Include(o => o.USER_ACCOUNTS);
            return View(orders.ToList());
        }

        // GET: /Orders/Details/5
        public ActionResult Details(int? id)
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

        // GET: /Orders/Create
        public ActionResult Create()
        {
            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_CITY");
            ViewBag.USER_ID = new SelectList(db.USER_ACCOUNTS, "USER_ID", "UserName");
            return View();
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ORDER_NUMBER,USER_ID,DATETIME_ORDER_PLACED,DEPARTURE_DATETIME,DEPARTURE_STREET_ADDRESS,DEPARTURE_CITY,DEPARTURE_STATE,DEPARTURE_ZIPCODE,DESTINATION_NAME,NUMBER_OF_PASSENGERS,FLIGHT_DETAILS,COMMENTS")] ORDER order)
        {
            if (ModelState.IsValid)
            {
                db.ORDERS.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_CITY", order.DESTINATION_NAME);
            ViewBag.USER_ID = new SelectList(db.USER_ACCOUNTS, "USER_ID", "UserName", order.USER_ID);
            return View(order);
        }

        // GET: /Orders/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_CITY", order.DESTINATION_NAME);
            ViewBag.USER_ID = new SelectList(db.USER_ACCOUNTS, "USER_ID", "UserName", order.USER_ID);
            return View(order);
        }

        // POST: /Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ORDER_NUMBER,USER_ID,DATETIME_ORDER_PLACED,DEPARTURE_DATETIME,DEPARTURE_STREET_ADDRESS,DEPARTURE_CITY,DEPARTURE_STATE,DEPARTURE_ZIPCODE,DESTINATION_NAME,NUMBER_OF_PASSENGERS,FLIGHT_DETAILS,COMMENTS")] ORDER order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DESTINATION_NAME = new SelectList(db.DESTINATIONS, "DESTINATION_NAME", "DESTINATION_CITY", order.DESTINATION_NAME);
            ViewBag.USER_ID = new SelectList(db.USER_ACCOUNTS, "USER_ID", "UserName", order.USER_ID);
            return View(order);
        }

        // GET: /Orders/Delete/5
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

        // POST: /Orders/Delete/5
        [HttpPost, ActionName("Delete")]
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
