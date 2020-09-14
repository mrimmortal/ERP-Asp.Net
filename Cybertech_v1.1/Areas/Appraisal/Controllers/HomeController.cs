using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cybertech_v1._1.Areas.Appraisal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Appraisal/Home
        [AccessControl]
        public ActionResult Index()
        {
            return View();
        }
        [AccessControl]
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}