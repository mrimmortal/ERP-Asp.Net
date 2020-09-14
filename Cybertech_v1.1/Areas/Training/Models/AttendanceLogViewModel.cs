using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cybertech_v1._1.Areas.Training.Models
{
    public class AttendanceLogViewModel
    {
            public int Attendance_Id { get; set; }
            public Nullable<int> CSSL_ID { get; set; }
            public Nullable<System.DateTime> LoginDateTime { get; set; }
            public string Attendance_Status { get; set; }        
            public string Remark { get; set; }
            public SelectList Attendance_Status_Dropdown_List { get; set; }          
            public List<Statuslist> Attendance_Status_List { get; set; }        
    }

    public class Statuslist
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Selected { get; set; }
    }

}
