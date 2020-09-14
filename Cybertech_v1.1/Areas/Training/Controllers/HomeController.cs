using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cybertech_v1._1.EntityDataContext;
using Cybertech_v1._1.Areas.Training.Models;
using System.Reflection;

namespace Cybertech_v1._1.Areas.Training.Controllers
{
    public class HomeController : Controller
    {
        CybertechEntities db = new CybertechEntities();
        clsErrorLogs objErrorLog = null;

        //GET: Training/Home
        public ActionResult Index()
        {
            return View();
        }
        [AccessControl]
        public ActionResult TraineeCRUD() // Trainne CRUD Operation View 
        {
            return View(db.Training_ActiveTrainees.Where(x=>x.Active=="Active").ToList());
        }
        public ActionResult AddTrainee()
        {
            return PartialView("_AddTrainee");
        }
        [HttpPost]
        public ActionResult AddTrainee(Training_ActiveTraineesModel obj)
        {
            if (ModelState.IsValid)
            {
                Training_ActiveTrainees addobj = new Training_ActiveTrainees() // new active Trainee
                {
                    CSSL_ID = obj.CSSL_ID,
                    Trainee_Name = obj.Trainee_Name,
                    Role = obj.Role,
                    Supervisor = obj.Supervisor,
                    Type = obj.Type,
                    Technology = obj.Technology,
                    DOJ = obj.DOJ,
                    Team = obj.Team,
                    Active = obj.Active,
                    Shift_Time = obj.Shift_Time
                };

                Core_Users coreUserAdd = new Core_Users() // Create User name for trainee login 
                {
                    user_id = obj.CSSL_ID,
                    user_name = obj.CSSL_Domain_User_Name
                };

                Core_LNK_User_Role coreLinkRole = new Core_LNK_User_Role() // Create role mapping for user
                {
                    user_id = obj.CSSL_ID,
                    role_id = db.Core_Roles.Where(x => x.role_name == obj.Role).Select(x => x.role_id).FirstOrDefault()
                };

                if (db.Training_ActiveTrainees.Where(x => x.CSSL_ID == addobj.CSSL_ID).Count() == 0)
                {
                    db.Training_ActiveTrainees.Add(addobj); //Add Trainee in Trainee Table 
                    db.SaveChanges();
                }
                if (db.Core_Users.Where(x => x.user_id == coreUserAdd.user_id).Count() == 0)
                {
                    db.Core_Users.Add(coreUserAdd); //Add User Name for Trainee ID For user Table 
                    db.SaveChanges();
                }
                if (db.Core_LNK_User_Role.Where(x => x.user_id == coreLinkRole.user_id).Count() == 0)
                {
                    db.Core_LNK_User_Role.Add(coreLinkRole); //Add Role For User 
                    db.SaveChanges();
                }
            }
            return PartialView("_TraineeDetail", db.Training_ActiveTrainees.ToList());
        }
        [HttpGet]
        public JsonResult IsCSSLExist(string CSSL_ID)
        {
            bool isExist = db.Training_ActiveTrainees.Where(u => u.CSSL_ID.Equals(CSSL_ID)) != null;
            return Json(!isExist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditTrainee(int id)
        {
            var data = db.Training_ActiveTrainees.Where(x => x.CSSL_ID == id).FirstOrDefault();
            return PartialView("_EditTrainee", data);
        }
        [HttpPost]
        public ActionResult EditTrainee(Training_ActiveTrainees obj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView("_TraineeDetail", db.Training_ActiveTrainees.ToList());
        }
        public ActionResult DeleteTrainee(int id)
        {

            var obj = db.Training_ActiveTrainees.Where(x => x.CSSL_ID == id).FirstOrDefault();

            var link_role = db.Core_LNK_User_Role.Where(x => x.user_id == obj.CSSL_ID).Select(x => x);
            if (link_role.Count() != 0)
            {
                db.Core_LNK_User_Role.Remove(link_role.FirstOrDefault()); //Add Role For User 
                db.SaveChanges();
            }

            var coreuser = db.Core_Users.Where(x => x.user_id == obj.CSSL_ID).Select(x => x);
            if (coreuser.Count() == 0)
            {
                db.Core_Users.Remove(coreuser.FirstOrDefault()); //Add User Name for Trainee ID For user Table 
                db.SaveChanges();
            }

            db.Training_ActiveTrainees.Remove(obj);
            db.SaveChanges();




            return PartialView("_TraineeDetail", db.Training_ActiveTrainees.ToList());
        }
        [AccessControl]
        public ActionResult TraineeAttendance() //Attendance PageLoad 
        {
            TraineeAttendanceModel t = new TraineeAttendanceModel();
            using (CybertechEntities db = new CybertechEntities())
            {
                try
                {
                    var User = HttpContext.User.Identity.Name.ToString().Trim();

                    var UserDetails = (from u in db.Core_Users
                                       join e in db.Training_ActiveTrainees on u.user_id equals e.CSSL_ID
                                       where u.user_name == User
                                       select e).FirstOrDefault();

                    var Status = (from s in db.Training_AttendanceLogs where s.CSSL_ID == UserDetails.CSSL_ID select s).OrderByDescending(m => m.Attendance_Id).FirstOrDefault();

                    DateTime LastLogin = Convert.ToDateTime(Status.LoginDateTime);

                    var Shift_Time = DateTime.ParseExact(UserDetails.Shift_Time.ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None).AddMinutes(5);

                    var Access_time = DateTime.ParseExact(UserDetails.Shift_Time.ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None).AddMinutes(-5);



                    if (LastLogin.Date != DateTime.Now.Date)
                    {
                        if (DateTime.Now.TimeOfDay > Shift_Time.TimeOfDay)
                        {
                            t.CSSL_ID = UserDetails.CSSL_ID;
                            t.Status = "D";
                            t.TraineeName = UserDetails.Trainee_Name;
                            t.AccessTime = Access_time;
                        }
                        else
                        {
                            t.CSSL_ID = UserDetails.CSSL_ID;
                            t.TraineeName = UserDetails.Trainee_Name;
                            t.Status = "A";
                            t.AccessTime = Access_time;
                        }
                    }
                    else
                    {
                        t.CSSL_ID = UserDetails.CSSL_ID;
                        t.Status = "P";
                        t.TraineeName = UserDetails.Trainee_Name;
                        t.AccessTime = Access_time;
                    }

                }
                catch (Exception ex)
                {
                    objErrorLog = new clsErrorLogs();
                    objErrorLog.WriteErrorLogs("Training", MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message.ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString());
                }
            }
            return View(t);
        }
        public ActionResult MarkAttendance(TraineeAttendanceModel e) //Mark Attendance for Trainee 
        {
            if (e != null && e.CSSL_ID != 0)
            {
                try
                {
                    if (e.Status == "A")
                    {
                        using (CybertechEntities db = new CybertechEntities())
                        {
                            Training_AttendanceLogs a = new Training_AttendanceLogs()
                            {
                                Attendance_Status = "P",
                                CSSL_ID = e.CSSL_ID,
                                LoginDateTime = DateTime.Now
                            };
                            db.Training_AttendanceLogs.Add(a);
                            db.SaveChanges();
                        }
                    }
                    if (e.Status == "D")
                    {
                        using (CybertechEntities db = new CybertechEntities())
                        {
                            Training_AttendanceLogs a = new Training_AttendanceLogs()
                            {
                                Attendance_Status = "D",
                                CSSL_ID = e.CSSL_ID,
                                LoginDateTime = DateTime.Now
                            };
                            db.Training_AttendanceLogs.Add(a);
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    objErrorLog = new clsErrorLogs();
                    objErrorLog.WriteErrorLogs("Training", MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message.ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString());
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [AccessControl]
        public ActionResult ViewAttendance() //Trainee View Load 
        {
            return View(TodaysAttendanceData());
        }
        public ViewAttendanceModel TodaysAttendanceData() //Todays Attendance Data
        {

            using (CybertechEntities db = new CybertechEntities())
            {

                ViewAttendanceModel va = new ViewAttendanceModel();
                try
                {
                    va.TraineesName = (from u in db.Training_ActiveTrainees
                                       select new Trainee
                                       {
                                           CSSL_ID = u.CSSL_ID,
                                           Name = u.Trainee_Name
                                       }).ToList();
                    var today = (DateTime?)DateTime.Now.Date;
                    ViewBag.Traineelist = new SelectList(va.TraineesName, "CSSL_ID", "Name");

                    var temp = (from al in db.Training_AttendanceLogs
                                join at in db.Training_ActiveTrainees
                                on al.CSSL_ID equals at.CSSL_ID
                                select new Attendancerecord
                                {
                                    TraineeName = at.Trainee_Name,
                                    Attendance_id = al.Attendance_Id,
                                    Date = (DateTime)al.LoginDateTime,
                                    Status = al.Attendance_Status
                                }).ToList();

                    va.Records = temp.Where(x => x.Date.Date == today).ToList();
                }
                catch (Exception ex)
                {
                    objErrorLog = new clsErrorLogs();
                    objErrorLog.WriteErrorLogs("Training", MethodInfo.GetCurrentMethod().DeclaringType.Name, MethodInfo.GetCurrentMethod().Name, ex.Message.ToString(), Request.ServerVariables["REMOTE_ADDR"].ToString());
                }
                return va;
            }
        }
        public ActionResult FilterAttendance(ViewAttendanceModel o)
        {
            using (CybertechEntities db = new CybertechEntities())
            {
                List<DateTime> range = new List<DateTime>();
                for (var dt = o.From; dt <= o.To; dt = dt.AddDays(1))
                {
                    range.Add(dt);
                }
                var CSSL_ID = int.Parse(o.CSSL_ID.ToString());
                var temp = (from al in db.Training_AttendanceLogs
                            where al.CSSL_ID == CSSL_ID
                            select new Attendancerecord
                            {
                                Attendance_id = al.Attendance_Id,
                                Date = (DateTime)al.LoginDateTime,
                                Status = al.Attendance_Status
                            }).ToList();

                var Records = temp.Where(x => x.Date.Date >= o.From.Date && x.Date.Date <= o.To.Date).ToList();

                var extra = (from al in db.Training_AttendanceLogs
                             where al.CSSL_ID == CSSL_ID && al.LoginDateTime >= o.From && al.LoginDateTime <= o.To
                             select al).ToList();
                foreach (var d in range)
                {
                    if (Records.Where(x => x.Date.Date == d.Date).Count() == 0)
                    {
                        if ((d.DayOfWeek == DayOfWeek.Saturday) || (d.DayOfWeek == DayOfWeek.Sunday))
                        {
                            Attendancerecord a = new Attendancerecord()
                            {
                                Attendance_id = -1,
                                Date = d.Date,
                                Status = "WO",
                                CSSL_ID = CSSL_ID
                            };
                            Records.Add(a);
                        }
                        else
                        {
                            Attendancerecord a = new Attendancerecord()
                            {
                                Attendance_id = -1,
                                Date = d.Date,
                                Status = "A",
                                CSSL_ID = CSSL_ID
                            };
                            Records.Add(a);
                        }
                    }
                }
                return PartialView("_AttendanceView", Records.OrderBy(x => x.Date));
            }
        }
        public ActionResult AttendanceRemark(int Attendance_id, int CSSL_ID, DateTime Date, String Status)
        {

            if (Attendance_id != -1)
            {
                var x = db.Training_AttendanceLogs.Find(Attendance_id);
                AttendanceLogViewModel log = new AttendanceLogViewModel()
                {
                    Attendance_Id = x.Attendance_Id,
                    CSSL_ID = x.CSSL_ID,
                    LoginDateTime = x.LoginDateTime,
                    Attendance_Status = x.Attendance_Status
                };

                var l = new List<Statuslist>
                {
                new Statuslist() { Text = "A", Value = "A", Selected = "" },
                new Statuslist() { Text = "D", Value = "D", Selected = "" },
                new Statuslist() { Text = "P", Value = "P", Selected = "" }
                };

                log.Attendance_Status_List = l;
                //log.Attendance_Status_Dropdown_List = new SelectList(log.Attendance_Status_List,"Value","Text");

                foreach (var item in log.Attendance_Status_List)
                {
                    if (item.Text == x.Attendance_Status)
                    {
                        item.Selected = "selected";
                        break;
                    }
                }
                return PartialView("_EditAttendanceLog", log);
            }

            else
            {
                AttendanceLogViewModel log = new AttendanceLogViewModel()
                {
                    Attendance_Id = Attendance_id,
                    CSSL_ID = CSSL_ID,
                    LoginDateTime = Date,
                    Attendance_Status = Status
                };
                var l = new List<Statuslist>
                {
                new Statuslist() { Text = "A", Value = "A", Selected = "" },
                new Statuslist() { Text = "D", Value = "D", Selected = "" },
                new Statuslist() { Text = "P", Value = "P", Selected = "" }
                 };

                log.Attendance_Status_List = l;

                //log.Attendance_Status_Dropdown_List = new SelectList(log.Attendance_Status_List,"Value","Text");

                foreach (var item in log.Attendance_Status_List)
                {
                    if (item.Text == Status)
                    {
                        item.Selected = "selected";
                        break;
                    }
                }
                return PartialView("_EditAttendanceLog", log);
            }
        }
        [HttpPost]
        public ActionResult AttendanceRemark(AttendanceLogViewModel obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Attendance_Id != -1)
                {
                    var x = db.Training_AttendanceLogs.Find(obj.Attendance_Id);
                    x.Attendance_Status = obj.Attendance_Status;
                    x.Remark = obj.Remark;
                    db.Entry(x).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    Training_AttendanceLogs a = new Training_AttendanceLogs()
                    {
                        Attendance_Status = obj.Attendance_Status,
                        CSSL_ID = obj.CSSL_ID,
                        LoginDateTime = obj.LoginDateTime
                    };
                    db.Training_AttendanceLogs.Add(a);
                    db.SaveChanges();
                }
            }

            if (obj.LoginDateTime.Value.Date == DateTime.Now.Date)
            {
                return PartialView("_TodayAttendanceView", TodaysAttendanceData().Records);
            }
            else
            {
                return Json(new { success = true, today = false }, JsonRequestBehavior.DenyGet);
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
