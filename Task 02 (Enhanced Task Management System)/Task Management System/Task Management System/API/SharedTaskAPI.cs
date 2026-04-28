using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Task_Management_System.Models;

namespace Task_Management_System.API
{
    [RoutePrefix("api/tasks")]
    public class SharedTaskAPIController : ApiController
    {
        ErrorLogMasterModel elm = new ErrorLogMasterModel();

        [HttpGet]
        [Route("GetAllSharedTasks")]
        public async Task<IHttpActionResult> GetAllSharedTasks()
        {
            try
            {
                SharedTaskAPIModel st = new SharedTaskAPIModel();
                var result = await st.GetSharedTask();
                return Ok(result);
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Current.Session["UserName"] == null ? "UnknownUser" : HttpContext.Current.Session["UserName"].ToString(),
                       "SharedTaskAPIController", "GetAllSharedTasks"
                   );
                return Json(new
                {
                    Status = "CatchError",
                    URL = "/ErrorLogMaster/Index"
                });
            }
        }

        [HttpPost]
        [Route("share")]
        public async Task<IHttpActionResult> ShareTask([FromBody] SharedTaskAPIModel model)
        {
            int Notification = 0;
            int result = 0; 
            try
            {
                if (ModelState.IsValid)
                {
                    result = await model.AddTaskShares();

                    if (result == 1)
                    {
                        Notification = await model.SendNotification(model.shareWithUserId);
                        if (Notification == 1)
                        {
                            return Json(new
                            {
                                Status = "Success",
                                Message = "Task Share successfully.",
                                //URL = "/Task/Index"
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Status = "Error",
                                Message = "Task shared but notification failed."
                            });
                        }
                    }
                    else if (result == 0)
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Task already shared.",
                           // URL = "/Task/Index"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Error",
                            Message = "Failed to share task. Please try again.",
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Status = "Error",
                        Message = "Failed.",
                    });
                }
            }
            catch (Exception ex)
            {
                elm.Add(
                       ex.Message == null ? "No Message" : ex.Message,
                       ex.InnerException == null ? "No Inner Exception" : ex.InnerException.Message,
                       DateTime.Now,
                       HttpContext.Current.Session["UserName"] == null ? "UnknownUser" : HttpContext.Current.Session["UserName"].ToString(),
                       "SharedTaskAPIController", "ShareTask"
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