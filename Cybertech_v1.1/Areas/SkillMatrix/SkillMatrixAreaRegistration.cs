using System.Web.Mvc;

namespace Cybertech_v1._1.Areas.SkillMatrix
{
    public class SkillMatrixAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SkillMatrix";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SkillMatrix",
                "SkillMatrix/{controller}/{action}/{id}",
                new { action = "Home", id = UrlParameter.Optional }
            );
        }
    }
}