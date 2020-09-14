using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cybertech_v1._1.Areas.Training.Models
{
    public class Training_ActiveTraineesModel
    {
        public int T_Uid { get; set; }
        public int CSSL_ID { get; set; }
        public string Trainee_Name { get; set; }
        public string CSSL_Domain_User_Name{ get; set; }
        public string Role { get; set; }
        public string Supervisor { get; set; }
        public string Type { get; set; }
        public string Technology { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string Team { get; set; }
        public string Active { get; set; }
        public Nullable<System.TimeSpan> Shift_Time { get; set; }
    }
}