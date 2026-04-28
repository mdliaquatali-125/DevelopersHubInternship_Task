using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Management_System.Models
{
    public class NotificationModel
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}