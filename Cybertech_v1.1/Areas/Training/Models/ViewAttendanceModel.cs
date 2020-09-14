using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cybertech_v1._1.Areas.Training.Models
{
    public class ViewAttendanceModel
    {
        public List<Trainee> TraineesName { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
        public string TraineeName { get; set; }
        public string CSSL_ID { get; set; }
        public List<Attendancerecord> Records { get; set; }        
    }
    public class Trainee
    {
        public string Name { get; set; }
        public int? CSSL_ID { get; set; }
    }
    public class Attendancerecord
    {
        public int Attendance_id { get; set; }
        public string TraineeName { get; set; }
        public DateTime Date { get; set; }
        public String Status { get; set; }
        public int CSSL_ID { get; set; }

        public string Statuscolor
        {
            get
            {
                if (Status == "A")
                {
                    return "bg-danger disabled color-palette";
                }
                else if (Status == "P")
                {
                    return "bg-success disabled color-palette";
                }
                else if (Status == "D")
                {
                    return "bg-warning disabled color-palette";
                }
                else
                {
                    return "bg-primary disabled color-palette";
                }
            }
           
        }
    }
}