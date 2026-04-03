
namespace Task_Management_System
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserMaster
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public string EditBy { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
        public string UserAddress { get; set; }
    }
}
