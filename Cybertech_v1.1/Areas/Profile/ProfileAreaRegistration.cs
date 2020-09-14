using System.Web.Mvc;

namespace Cybertech_v1._1.Areas.Profile
{
    public class ProfileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Profile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Profile",
                "Profile/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index",  id = UrlParameter.Optional }
            );
        }
    }
}