using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetFrameworkSample.Controllers
{
    public class HomeController : Controller
    {
        private string greeting = "";
        private int majorDotNetVersion = 0;
        public HomeController()
        {
            greeting = ConfigurationManager.AppSettings["Greeting"];
            majorDotNetVersion = Int32.Parse(ConfigurationManager.AppSettings["CurrentMajorDotNetVersion"]);
        }
        public ActionResult Index()
        {
            ViewBag.Greeting = greeting;
            ViewBag.MajorDotNetVersion = majorDotNetVersion;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}