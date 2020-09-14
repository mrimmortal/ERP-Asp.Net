using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cybertech_v1._1.Areas.Training.Models
{
    public class TraineeAttendanceModel
    {
        public int CSSL_ID { get; set; }
        public String TraineeName { get; set; }
        public string Status { get; set; }
        public DateTime AccessTime { get; set; }
    }
}