using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Task_Management_System.SignalR;

namespace Task_Management_System.Models
{
    public class SharedTaskAPIModel
    {
        public int TaskShareID { get; set; }
        public int TaskID { get; set; }
        public int shareWithUserId { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public string Title { get;  set; }
        public string Description { get;  set; }
        public string Status { get;  set; }
        public DateTime DueDate { get;  set; }
        public DateTime SharedDate { get;  set; }
        public string Username { get;  set; }
        public string UploadFile { get;  set; }

        TaskManagementSystemEntities db = new TaskManagementSystemEntities();

        public async Task<int> AddTaskShares()
        {
            int result = 0;

            var today = DateTime.Today;

            var exist = await db.TaskShares.Where(x => x.TaskID == TaskID && x.shareWithUserId == shareWithUserId && DbFunctions.TruncateTime(x.CreatedDate) == today).CountAsync();

            if (exist > 0)
            {
                return 0; // already shared today
            }

            TaskShare t = new TaskShare();

            t.TaskID = TaskID;
            t.shareWithUserId = shareWithUserId;
            t.CreatedDate = DateTime.Now;
            t.CreatedBy = HttpContext.Current.Session["UserName"] == null ? "UnknownUser" : HttpContext.Current.Session["UserName"].ToString();
            t.IsActive = true;
            db.TaskShares.Add(t);
            await db.SaveChangesAsync();
            result = 1;
            return result;
        }

        public async Task<int> SendNotification(int shareWithUserId)
        {
            var GetuserName = await db.UserMasters.Where(x => x.UserID == shareWithUserId).Select(x => x.UserName).FirstOrDefaultAsync();

            var SharedUserName = HttpContext.Current.Session["UserName"] == null ? "UnknownUser" : HttpContext.Current.Session["UserName"].ToString();

            Notification n = new Notification
            {
                UserID = shareWithUserId,
                Message = "Hello " + GetuserName + ", " + SharedUserName + " has shared a task with you. Please check your shared tasks for details.",
                CreatedDate = DateTime.Now,
            };

            db.Notifications.Add(n);
            await db.SaveChangesAsync();

            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            context.Clients.User(shareWithUserId.ToString())
                .receiveNotification(new
                {
                    title = "Task Shared",
                    message = n.Message,
                    time = DateTime.Now.ToString("hh:mm tt")
                });

            return 1;
        }

        public async Task<int> SendNotifications(int TaskID)
        {

            var GetshareWithUserId = await db.TaskShares.Where(x => x.TaskID == TaskID).Select(x => x.shareWithUserId).FirstOrDefaultAsync();

            if (GetshareWithUserId == 0 || GetshareWithUserId == null)
            {
                return 0;
            }
            else
            {

                  var GetuserName = await db.UserMasters.Where(x => x.UserID == GetshareWithUserId).Select(x => x.UserName).FirstOrDefaultAsync();

                  var UserName = HttpContext.Current.Session["UserName"] == null ? "UnknownUser" : HttpContext.Current.Session["UserName"].ToString();

                    Notification n = new Notification
                    {
                        UserID = GetshareWithUserId,
                        Message = "Hello " + GetuserName + ", " + UserName + " has updated a task status.",
                        CreatedDate = DateTime.Now,
                    };

                    db.Notifications.Add(n);
                    await db.SaveChangesAsync();

                    var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                    context.Clients.User(GetshareWithUserId.ToString())
                        .receiveNotification(new
                        {
                            title = "Task Updated",
                            message = n.Message,
                            time = DateTime.Now.ToString("hh:mm tt")
                        });
                    return 1;
            }
        }

        public async Task<List<SharedTaskAPIModel>> GetSharedTask()
        {
            List<SharedTaskAPIModel> List = await (from ts in db.TaskShares
                                                          join t in db.Tasks 
                                                                on ts.TaskID equals t.TaskID
                                                                        join um in db.UserMasters
                                                                                on ts.shareWithUserId equals um.UserID
                                                                                        where ts.IsActive == true
                                                                                                select new SharedTaskAPIModel
                                                                                                     {
                                                                                                         TaskShareID = ts.TaskShareID,
                                                                                                         TaskID = (int)ts.TaskID,
                                                                                                         shareWithUserId = (int)ts.shareWithUserId,
                                                                                                         IsActive = (bool)ts.IsActive,
                                                                                                         SharedDate = (DateTime)ts.CreatedDate,
                                                                                                         Title = t.Title,
                                                                                                         Description = t.Description,
                                                                                                         Status = t.Status,
                                                                                                         DueDate = (DateTime)t.DueDate,
                                                                                                         Username = um.UserName,
                                                                                                         CreatedBy = ts.CreatedBy,
                                                                                                         UploadFile = t.UploadFile
                                                                                                     }
                    ).ToListAsync();
            return List;
        }

    }
}