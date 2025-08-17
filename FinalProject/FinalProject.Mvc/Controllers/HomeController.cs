using FinalProject.Business;
using FinalProject.Data;
using FinalProject.Mvc.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.History;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace FinalProject.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositoryTasks repositoryTasks = new RepositoryTasks();
        private readonly RepositoryUserTasks repositoryUserTask = new RepositoryUserTasks();

        [HttpGet]
        public ActionResult Index()
        {
            int userId = GetCurrentUserIdFromSession();

            var allTasks = repositoryTasks.GetAll(); 
            var userTasks = repositoryUserTask.GetAll()
                .Where(ut => ut.UserId == userId)
                .ToList(); 

          
            var userTaskDetails = userTasks.Select(ut => new UserTaskView
            {
                UserTaskId = ut.UserTaskId,
                TaskId = ut.TaskId,
                TaskName = allTasks.FirstOrDefault(t => t.TaskId == ut.TaskId)?.TaskName ?? "Unknown",
                Result = ut.Result
            }).ToList();

            var model = new DashboardView
            {
                AllTasks = allTasks.ToList(),
                UserTasks = userTaskDetails
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddTask(int taskId, int chart, int interval)
        {
            int userId = GetCurrentUserIdFromSession();


            bool exists = repositoryUserTask.GetAll()
                .Any(ut => ut.UserId == userId && ut.TaskId == taskId);

            if (!exists)
            {
                var userTask = new UserTask
                {
                    UserId = userId,
                    TaskId = taskId,
                    Chart = chart,
                    Status = "Pending",
                    RepeatIntervalHours = interval,
                    ExecutionDate = DateTime.Now
                };

                repositoryUserTask.Add(userTask);
                repositoryUserTask.Save();
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult RemoveTask(int taskId)
        {
            int userId = GetCurrentUserIdFromSession();

            var userTask = repositoryUserTask.GetAll()
                .FirstOrDefault(ut => ut.UserId == userId && ut.TaskId == taskId);

            if (userTask != null)
            {
                repositoryUserTask.Delete(userTask.UserTaskId);
                repositoryUserTask.Save();
            }

            return RedirectToAction("Index");
        }

        private int GetCurrentUserIdFromSession()
        {
            return (int)Session["UserId"];
        }
        [HttpGet]
        public JsonResult GetUserTasks()
        {
            int userId = GetCurrentUserIdFromSession();

            var userTasks = repositoryUserTask.GetAll()
                .Where(ut => ut.UserId == userId)
                .Select(ut => new {
                    ut.UserTaskId,
                    ut.TaskId,
                    ut.Result,
                    ChartType = ut.Chart,
                    TaskName = repositoryTasks.GetById(ut.TaskId)?.TaskName ?? "N/A",
                   History = ut.UserTaskResults.OrderByDescending(utr => utr.ExecutionDate).Select(utr => new {
                       value = utr.ResultValue,
                       date = utr.ExecutionDate.ToString("yyyy-MM-dd HH:mm:ss")
                   }).Take(10).ToList()
                })
                .ToList();

            return Json(userTasks, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexDash()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Index", "Account");
            }

            ViewBag.Username = Session["User"] != null ? Session["User"].ToString() : "Invitado";
            ViewBag.Role = Session["RoleId"] != null ? Session["RoleId"].ToString() : "Sin Rol";


            return View();
        }
    }
}