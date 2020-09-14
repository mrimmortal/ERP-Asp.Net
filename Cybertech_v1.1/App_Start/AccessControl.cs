using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Cybertech_v1._1.EntityDataContext;
using System.Web.UI;

namespace Cybertech_v1._1
{
    public class AccessControl : ActionFilterAttribute
    {
        clsErrorLogs objErrorLog = null;
        CybertechEntities db = new CybertechEntities();
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var rou = filterContext.RouteData;
                var des = filterContext.ActionDescriptor;
                var actionName = des.ActionName;
                var controllerName = des.ControllerDescriptor.ControllerName;
                var area = rou.DataTokens["area"] as string;
                var identity = HttpContext.Current.User.Identity.Name.ToString().Trim();
                UserAuth ss = new UserAuth();
                ss = (UserAuth)HttpContext.Current.Session["ID"];

                if (ss == null)
                {
                    UserAuth user = new UserAuth();
                    user = (from u in db.Core_Users
                            join lr in db.Core_LNK_User_Role on u.user_id equals lr.user_id
                            join r in db.Core_Roles on lr.role_id equals r.role_id
                            where u.user_name == identity
                            select new UserAuth { User_id = u.user_id, User_name = u.user_name, Role_id = r.role_id, Role_name = r.role_name ,Is_Admin = r.sys_admin }).FirstOrDefault();

                    user.Permissions = (from rmf in db.Core_LNK_Role_Module_Functionality
                                        join am in db.Core_Areas_Modules on rmf.module_id equals am.id
                                        join mf in db.Core_Modules_Functionality on rmf.functionality_id equals mf.id
                                        where rmf.role_id == user.Role_id
                                        select new Access { ModuleName = am.moduleName, FunctionalityName = mf.module_action_name }).ToList();

                    filterContext.HttpContext.Session.Add("ID", user);

                    if ((from a in user.Permissions where a.ModuleName == area && a.FunctionalityName == actionName select a).Count() == 0)
                    {
                        if (area != null)
                        {
                            filterContext
                                .HttpContext
                                .Response
                                .RedirectToRoute(area, new { controller = controllerName, action = "Error" });
                        }
                        else
                        {
                            filterContext
                                .HttpContext
                                .Response
                                .RedirectToRoute("Default", new { controller = "Welcome", action = "Error" });
                        }
                    }
                }
                else
                {
                    if ((from a in ss.Permissions where a.ModuleName == area && a.FunctionalityName == actionName select a).Count() == 0)
                    {
                        if (area != null)
                        {
                            filterContext
                                .HttpContext
                                .Response
                                .RedirectToRoute(area, new { controller = controllerName, action = "Error" });
                        }
                        else
                        {
                            filterContext
                                .HttpContext
                                .Response
                                .RedirectToRoute("Default", new { controller = "Welcome", action = "Error" });
                        }
                    }
                }
                base.OnActionExecuted(filterContext);

            }

            catch (Exception ex)
            {
                objErrorLog = new clsErrorLogs();
                objErrorLog.WriteErrorLogs("analysisvolume", MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message.ToString(), HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());

            }
        }

    }
    public class UserAuth
    {
        public int User_id { get; set; }
        public string User_name { get; set; }
        public int Role_id { get; set; }
        public string Role_name { get; set; }
        public int? Is_Admin { get; set; }
        public List<Access> Permissions { get; set; }
    }
    public class Access
    {
        public string ModuleName { get; set; }
        public string FunctionalityName { get; set; }
    }

}