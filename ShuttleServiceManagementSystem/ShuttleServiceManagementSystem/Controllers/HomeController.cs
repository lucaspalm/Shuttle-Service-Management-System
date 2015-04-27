using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ShuttleServiceManagementSystem.Models;
using ShuttleServiceManagementSystem.Utilities;

namespace ShuttleServiceManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public SSMS_Helper ssms = new SSMS_Helper();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}