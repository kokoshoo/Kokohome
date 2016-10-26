using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Portfolio()
        {
            ViewBag.Message = "My Portfolio";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Reach out";

            return View();
        }
    }
}