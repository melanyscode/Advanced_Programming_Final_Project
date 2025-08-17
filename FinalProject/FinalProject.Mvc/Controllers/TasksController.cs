using FinalProject.Mvc.Filters;
using FinalProject.Business;
using FinalProject.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Mvc.Controllers
{
    public class TasksController : Controller
    {
        private readonly TasksBusiness taskBusiness;
        public TasksController()
        {
            taskBusiness = new TasksBusiness();
        }

        // GET: Tasks
        public ActionResult Index()
        {
            // Creacion de ViewBag para guardar el rol del usuario y esconder el boton de crear si no es admin
            var role = (Session["RoleId"] ?? "").ToString();
            ViewBag.IsAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        
            return View(taskBusiness.GetTasks());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = taskBusiness.GetById((int) id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        [RoleFilter("Admin")]
        public ActionResult Create()
        {

            ViewBag.PriorityOptions = new List<SelectListItem>
            {
                new SelectListItem { Text = "High", Value = "High" },
                new SelectListItem { Text = "Medium", Value = "Medium" },
                new SelectListItem { Text = "Low", Value = "Low" }
            };


            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleFilter("Admin")]
        public ActionResult Create([Bind(Include = "TaskId,TaskName,Priority,Executable")] Tasks tasks, HttpPostedFileBase file)
        {

            //route the uploaded file if any
            if (file != null && file.ContentLength > 0)
            {
                string path = @"C:\Scripts";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filePath = Path.Combine(path, Path.GetFileName(file.FileName));
                file.SaveAs(filePath);


                tasks.Executable = Path.GetFileName(file.FileName);
            }
            if (ModelState.IsValid)
            {
                tasks.CreatedAt = DateTime.Now;
                taskBusiness.SaveTask(0, tasks);
                taskBusiness.SaveChanges();
                return RedirectToAction("Index");
            }
            var executionDateErrors = ModelState["ExecutionDate"]?.Errors;
            if (executionDateErrors != null && executionDateErrors.Count > 0)
            {
                foreach (var error in executionDateErrors)
                {
                    System.Diagnostics.Debug.WriteLine("ExecutionDate error: " + error.ErrorMessage);
                }
            }

            return View(tasks);
        }


        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = taskBusiness.GetById((int)id);
            if (tasks == null)
            {
                return HttpNotFound();
            }

            ViewBag.PriorityOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "High", Value = "High" },
                    new SelectListItem { Text = "Medium", Value = "Medium" },
                    new SelectListItem { Text = "Low", Value = "Low" }
                };

            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaskId,TaskName,Priority,Executable,CreatedAt")] Tasks tasks)
        {
         

            if (ModelState.IsValid)
            {
                tasks.UpdatedAt = DateTime.Now; 
                taskBusiness.Update(tasks);
                taskBusiness.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(tasks);
        }


        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = taskBusiness.GetById((int)id); 
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks tasks = taskBusiness.GetById((int)id);
            taskBusiness.DeleteTask((int) id);
            taskBusiness.SaveChanges();
            return RedirectToAction("Index");
        }
        /*

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        */
    }
}
