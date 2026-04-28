//using System;
//using System.Linq;
//using System.Web;
//using System.Web.Http;

//namespace Task_Management_System.API
//{
//    public class NotificationGetAPI : ApiController
//    {
//        // GET: NotificationGetAPI
//        [HttpGet]
//        [Route("api/notificat12222ions")]
//        public IHttpActionResult GetNotifications()
//        {
//            TaskManagementSystemEntities db  = new TaskManagementSystemEntities();
//            int userId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

//            var data = db.Notifications
//                .Where(x => x.UserID == userId)
//                .OrderByDescending(x => x.CreatedDate)
//                .Take(20)
//                .ToList();

//            return Ok(data);
//        }
//    }
//}