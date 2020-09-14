using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cybertech_v1._1.Areas.Training.Models;
using Cybertech_v1._1.Areas.Training.Models.ReportModels;
using Cybertech_v1._1.EntityDataContext;
using Attendancerecord = Cybertech_v1._1.Areas.Training.Models.ReportModels.Attendancerecord;
using Trainee = Cybertech_v1._1.Areas.Training.Models.ReportModels.Trainee;

namespace Cybertech_v1._1.Areas.Training.Controllers
{
    public class ReportController : Controller
    {
        // GET: Training/Report
        [AccessControl]
        public ActionResult TraineeAttendanceReport()
        {
            try
            {
                ReportTraineeAttendanceModel va = new ReportTraineeAttendanceModel();
                using (CybertechEntities db = new CybertechEntities())
                {
                    var dateString = Convert.ToDateTime(DateTime.Now.ToString());
                    var firstDayOfMonth = new DateTime(dateString.Year, dateString.Month, 1).Date;
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Date;

                    var temp = (from al in db.Training_AttendanceLogs
                                select new Attendancerecord
                                {
                                    Attendance_id = al.Attendance_Id,
                                    Date = (DateTime)al.LoginDateTime,
                                    Status = al.Attendance_Status,
                                    CSSL_ID = (int)al.CSSL_ID
                                    
                                }).ToList();

                    var Trainees = temp.Where(x => x.Date.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth ).GroupBy(x=>x.CSSL_ID).ToList();            
                    
                    List<DateTime> range = new List<DateTime>();
                   
                    for (var dt = firstDayOfMonth; dt <= lastDayOfMonth; dt = dt.AddDays(1))
                    {
                        range.Add(dt.Date);
                    }

                    ViewBag.range = range;                    
           
                    List<ReportTraineeNamesModel> sampleReportTraineeNamesModel = new List<ReportTraineeNamesModel>();
                    List<Trainee> sampleTraineeNames = new List<Trainee>();
                    List<string> sampleTechnologyName = new List<string>();

                    foreach (var trainee in Trainees)
                    {
                        ReportTraineeNamesModel Tn = new ReportTraineeNamesModel();
                        var T = db.Training_ActiveTrainees.Where(x => x.CSSL_ID == trainee.Key).FirstOrDefault();
                        Tn.CSSL_ID = "CSSL" + T.CSSL_ID;
                        Tn.TraineeName = T.Trainee_Name;
                        Trainee t = new Trainee() { CSSL_ID = T.CSSL_ID, Name = T.Trainee_Name };
                        sampleTraineeNames.Add(t);

                        if (!sampleTechnologyName.Contains(T.Technology))
                        {
                            sampleTechnologyName.Add(T.Technology);
                        }

                        List<Attendancerecord> sampleAttendanerecord = new List<Attendancerecord>();
                
                        foreach (var d in range)
                        {
                            var dr = trainee.Where(x =>((DateTime)x.Date).Date == d.Date);
                            if (dr.Count() == 0)
                            {
                                if ((d.DayOfWeek == DayOfWeek.Saturday) || (d.DayOfWeek == DayOfWeek.Sunday))
                                {

                                    Attendancerecord a = new Attendancerecord()
                                    {
                                        Attendance_id = -1,
                                        Date = d.Date,
                                        Status = "WO",
                                        //CSSL_ID = CSSL_ID
                                    };
                                    sampleAttendanerecord.Add(a);
                                }
                                else
                                {
                                    Attendancerecord a = new Attendancerecord()
                                    {
                                        Attendance_id = -1,
                                        Date = d.Date,
                                        Status = "A",
                                        //CSSL_ID = CSSL_ID
                                    };
                                    sampleAttendanerecord.Add(a);
                                }
                            }
                            else
                            {
                                sampleAttendanerecord.Add(dr.Select(x=>x).FirstOrDefault());
                            }
                        }
                        Tn.DateStatus = sampleAttendanerecord;
                        sampleReportTraineeNamesModel.Add(Tn);
                    }

                    va.Attendance = sampleReportTraineeNamesModel;
                    va.TraineeNames = sampleTraineeNames;

                    va.TechnologyNames = sampleTechnologyName;

                    ViewBag.Traineelist = new SelectList(va.TraineeNames, "CSSL_ID", "Name");
                    ViewBag.TechnologyList = new SelectList(va.TechnologyNames);

                  
                }
                return View("ReportTraineeAttendance", va);
            }
            catch (Exception e)
            {
                return View("ReportTraineeAttendance");
            }
        }

        public ActionResult FilterAttendance(ReportTraineeAttendanceModel obj)
        {
            try
            {
                using (CybertechEntities db = new CybertechEntities())
                {
                    var dateString = Convert.ToDateTime(obj.MonthAndYear.ToString());
                    var firstDayOfMonth = new DateTime(dateString.Year, dateString.Month, 1).Date;
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1).Date;

                    var temp = (from al in db.Training_AttendanceLogs join at in db.Training_ActiveTrainees 
                                on al.CSSL_ID equals at.CSSL_ID
                                select new Attendancerecord
                                {
                                    Attendance_id = al.Attendance_Id,
                                    Date = (DateTime)al.LoginDateTime,
                                    Status = al.Attendance_Status,
                                    CSSL_ID = (int)al.CSSL_ID,
                                    Technology = at.Technology

                                }).ToList();

                   
                                      
                    var Trainees = temp.Where(x => x.Date.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth).GroupBy(x => x.CSSL_ID).ToList();

                    if (obj.TraineeName != null)
                    {
                        var CSSL_ID = Int32.Parse(obj.TraineeName);
                        Trainees = temp.Where(x => x.Date.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth && x.CSSL_ID == CSSL_ID).GroupBy(x => x.CSSL_ID).ToList();
                    }
                    else if (obj.TechnologyNames != null)
                    {
                        Trainees = (from t in temp join ob in obj.TechnologyNames on t.Technology equals ob where t.Date.Date >= firstDayOfMonth && t.Date <= lastDayOfMonth select t).GroupBy(x => x.CSSL_ID).ToList();
                    }
                    else 
                    {
                        Trainees = temp.Where(x => x.Date.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth).GroupBy(x => x.CSSL_ID).ToList();
                    }

                    List<DateTime> range = new List<DateTime>();

                    for (var dt = firstDayOfMonth; dt <= lastDayOfMonth; dt = dt.AddDays(1))
                    {

                        range.Add(dt.Date);
                    }
                    ViewBag.range = range;

                    ReportTraineeAttendanceModel va = new ReportTraineeAttendanceModel();
                    List<ReportTraineeNamesModel> sampleReportTraineeNamesModel = new List<ReportTraineeNamesModel>();
                    foreach (var trainee in Trainees)
                    {
                        ReportTraineeNamesModel Tn = new ReportTraineeNamesModel();
                        var T = db.Training_ActiveTrainees.Where(x => x.CSSL_ID == trainee.Key).FirstOrDefault();
                        Tn.CSSL_ID = "CSSL" + T.CSSL_ID;
                        Tn.TraineeName = T.Trainee_Name;

                        List<Attendancerecord> sampleAttendanerecord = new List<Attendancerecord>();
                        foreach (var d in range)
                        {
                            var dr = trainee.Where(x => ((DateTime)x.Date).Date == d.Date);
                            if (dr.Count() == 0)
                            {
                                if ((d.DayOfWeek == DayOfWeek.Saturday) || (d.DayOfWeek == DayOfWeek.Sunday))
                                {

                                    Attendancerecord a = new Attendancerecord()
                                    {
                                        Attendance_id = -1,
                                        Date = d.Date,
                                        Status = "WO",
                                        //CSSL_ID = CSSL_ID
                                    };
                                    sampleAttendanerecord.Add(a);
                                }
                                else
                                {
                                    Attendancerecord a = new Attendancerecord()
                                    {
                                        Attendance_id = -1,
                                        Date = d.Date,
                                        Status = "A",
                                        //CSSL_ID = CSSL_ID
                                    };
                                    sampleAttendanerecord.Add(a);
                                }
                            }
                            else
                            {
                                sampleAttendanerecord.Add(dr.Select(x => x).FirstOrDefault());
                            }
                        }
                        Tn.DateStatus = sampleAttendanerecord;
                        sampleReportTraineeNamesModel.Add(Tn);
                    }
                    //va.Attendance = sampleReportTraineeNamesModel;
                    return PartialView("_ReportAttendanceListView", sampleReportTraineeNamesModel);
                }
            }
            catch (Exception e)
            {
                return View("_ReportAttendanceListView");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}