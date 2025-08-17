using FinalProject.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Business;
using FinalProject.Data;

namespace FinalProject.Mvc.Controllers
{
    public class UserTasksController : Controller
    {
        private readonly UserTasksBusiness userTasksBusiness;
        private readonly UserBusiness userBusiness;
        private readonly TasksBusiness tasksBusiness;

        public UserTasksController()
        {
            userTasksBusiness = new UserTasksBusiness();
            userBusiness = new UserBusiness();
            tasksBusiness = new TasksBusiness();
        }

        // GET: UserTasks
        public ActionResult Index()
        {
            // Creacion de ViewBag para guardar el rol del usuario y esconder el boton de crear si no es admin
            var role = (Session["RoleId"] ?? "").ToString();
            ViewBag.IsAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            
            var userTasks = userTasksBusiness.GetUserTasks();
            return View(userTasks);
        }

        // GET: UserTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userTask = userTasksBusiness.GetById(id.Value);
            if (userTask == null)
                return HttpNotFound();

            return View(userTask);
        }

        // GET: UserTasks/Create
        [RoleFilter("Admin")]
        public ActionResult Create()
        {
            ViewBag.TaskId = new SelectList(tasksBusiness.GetTasks(), "TaskId", "TaskName");
            ViewBag.UserId = new SelectList(userBusiness.GetUser(), "UserId", "Username");
            return View();
        }

        // POST: UserTasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleFilter("Admin")]
        public ActionResult Create([Bind(Include = "UserTaskId,TaskId,UserId,Status,Result,ExecutionDate")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                userTasksBusiness.SaveUserTask(0, userTask); 
                userTasksBusiness.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskId = new SelectList(tasksBusiness.GetTasks(), "TaskId", "TaskName", userTask.TaskId);
            ViewBag.UserId = new SelectList(userBusiness.GetUser(), "UserId", "Username", userTask.UserId);
            return View(userTask);
        }

        // GET: UserTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userTask = userTasksBusiness.GetById(id.Value);
            if (userTask == null)
                return HttpNotFound();

            ViewBag.TaskId = new SelectList(tasksBusiness.GetTasks(), "TaskId", "TaskName", userTask.TaskId);
            ViewBag.UserId = new SelectList(userBusiness.GetUser(), "UserId", "Username", userTask.UserId);
            return View(userTask);
        }

        // POST: UserTasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserTaskId,TaskId,UserId,Status,Result,ExecutionDate")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                userTasksBusiness.SaveUserTask(userTask.UserTaskId, userTask);
                userTasksBusiness.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskId = new SelectList(tasksBusiness.GetTasks(), "TaskId", "TaskName", userTask.TaskId);
            ViewBag.UserId = new SelectList(userBusiness.GetUser(), "UserId", "Username", userTask.UserId);
            return View(userTask);
        }

        // GET: UserTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userTask = userTasksBusiness.GetById(id.Value);
            if (userTask == null)
                return HttpNotFound();

            return View(userTask);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userTasksBusiness.DeleteUserTask(id);
            userTasksBusiness.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
