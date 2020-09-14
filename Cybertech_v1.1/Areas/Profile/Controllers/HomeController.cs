using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cybertech_v1._1.EntityDataContext;

namespace Cybertech_v1._1.Areas.Profile.Controllers
{
    public class HomeController : Controller
    {
        [AccessControl]
        public ActionResult Index()
        {
            //UserAuth a = (UserAuth)Session["ID"];
            //var role = a.role_name;
            return View();
        }
        public ActionResult Myteam()
        {
            return View();
        }
        public PartialViewResult addItem()
        {
          return PartialView("PartialViews/Myview");//view name optional if partial view name is addItem
           
        }
    }
}