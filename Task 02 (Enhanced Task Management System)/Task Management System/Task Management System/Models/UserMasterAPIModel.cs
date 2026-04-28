using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Task_Management_System.Models
{
    public class UserMasterAPIModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime EditDate { get; set; }
        public string EditBy { get; set; }
        public string UserEmail { get; set; }
        public string UserContact { get; set; }
        public string UserAddress { get; set; }

        TaskManagementSystemEntities db = new TaskManagementSystemEntities();

        public async Task<List<UserMasterAPIModel>> GetUserList()
        {
            List<UserMasterAPIModel> List = await (from um in db.UserMasters
                                             where um.IsActive == true
                                             select new UserMasterAPIModel
                                             {
                                                 UserID = um.UserID,
                                                 UserName = um.UserName,
                                                 IsActive = (bool)um.IsActive
                                             }
                    ).ToListAsync();
            return List;
        }

    }
}