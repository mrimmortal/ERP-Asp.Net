using System.Web.Mvc;

namespace Cybertech_v1._1.Areas.Appraisal
{
    public class AppraisalAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get
            {
                return "Appraisal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Appraisal",
                "Appraisal/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}