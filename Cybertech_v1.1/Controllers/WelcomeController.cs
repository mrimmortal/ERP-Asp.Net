using Cybertech_v1._1.EntityDataContext;
using Cybertech_v1._1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cybertech_v1._1.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: Wellcome       
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

        public ActionResult NewRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewRegistration(RegistrationFormModel s)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            CybertechEntities db = new CybertechEntities();
            Core_Users u = new Core_Users();

          //var fields = (from f in db.Profile_Fields select f).ToList();
            var fields = db.Profile_Fields;

            var ModelFieldList = s.GetType().GetProperties().Select(p => p).ToList();

            foreach (var f in fields)
            {
                Profile_Field_Value pv = new Profile_Field_Value
                {
                    field_id = f.field_id
                };
                var pro = (from fv in ModelFieldList where fv.Name == f.field_name select fv).FirstOrDefault();
                pv.field_value =  pro.ToString() ;
            }

            if (ModelState.IsValid)
            {
                var a = s.Gender;
                var b = s.FirstName;
                var c = s.Employer;
        
                ViewBag.result = "Hi  Thanks for the details. a mail will be sent to with all the login details ";
                return Content("<div class='alert alert-success'> Thanks!  updated successfully </ div >");
            }

            return View(s);
        }
    }
}
