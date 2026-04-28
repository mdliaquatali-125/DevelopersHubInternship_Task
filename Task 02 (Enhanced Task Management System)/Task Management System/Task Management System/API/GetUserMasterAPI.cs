using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Task_Management_System.Models;

namespace Task_Management_System.API   // 🔥 YE MUST HAI
{
 
    
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();
        //✅ GET all tasks
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IHttpActionResult> GetAllUser()
        {
            try
            {
                UserMasterAPIModel um = new UserMasterAPIModel();
                var result = await um.GetUserList();
                return Ok(new
                {
                    Status = "Success",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Current.Session["UserName"] == null ? "UnknownUser" : HttpContext.Current.Session["UserName"].ToString(),
                       "UserController", "GetAllUser"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }
    }
}