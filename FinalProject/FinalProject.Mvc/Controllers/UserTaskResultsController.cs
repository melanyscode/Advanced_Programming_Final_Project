using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using FinalProject.Data;
using Repository; 

namespace FinalProject.Mvc.Controllers
{
    public class UserTaskResultsController : Controller
    {
        private readonly IRepositoryUserTaskResults _userTaskResultsRepository;
        private readonly IRepositoryUserTasks _userTasksRepository;

        public UserTaskResultsController()
        {
            _userTaskResultsRepository = new RepositoryUserTaskResults();
            _userTasksRepository = new RepositoryUserTasks();
        }

        // GET: UserTaskResults
        public ActionResult Index()
        {
            var results = _userTaskResultsRepository.GetAll();
            return View(results);
        }

        // GET: UserTaskResults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userTaskResult = _userTaskResultsRepository.GetById(id.Value);
            if (userTaskResult == null) return HttpNotFound();

            return View(userTaskResult);
        }

        // GET: UserTaskResults/Create
        public ActionResult Create()
        {
            var userTasks = _userTasksRepository.GetAll();
            ViewBag.UserTaskId = new SelectList(userTasks, "UserTaskId", "Status");
            return View();
        }

        // POST: UserTaskResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ResultId,UserTaskId,ExecutionDate,ResultValue")] UserTaskResults userTaskResults)
        {
            if (ModelState.IsValid)
            {
                _userTaskResultsRepository.Add(userTaskResults);
                _userTaskResultsRepository.Save();
                return RedirectToAction("Index");
            }

            var userTasks = _userTasksRepository.GetAll();
            ViewBag.UserTaskId = new SelectList(userTasks, "UserTaskId", "Status", userTaskResults.UserTaskId);
            return View(userTaskResults);
        }

        // GET: UserTaskResults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userTaskResult = _userTaskResultsRepository.GetById(id.Value);
            if (userTaskResult == null) return HttpNotFound();

            var userTasks = _userTasksRepository.GetAll();
            ViewBag.UserTaskId = new SelectList(userTasks, "UserTaskId", "Status", userTaskResult.UserTaskId);
            return View(userTaskResult);
        }

        // POST: UserTaskResults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResultId,UserTaskId,ExecutionDate,ResultValue")] UserTaskResults userTaskResults)
        {
            if (ModelState.IsValid)
            {
                _userTaskResultsRepository.Update(userTaskResults);
                _userTaskResultsRepository.Save();
                return RedirectToAction("Index");
            }

            var userTasks = _userTasksRepository.GetAll();
            ViewBag.UserTaskId = new SelectList(userTasks, "UserTaskId", "Status", userTaskResults.UserTaskId);
            return View(userTaskResults);
        }

        // GET: UserTaskResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userTaskResult = _userTaskResultsRepository.GetById(id.Value);
            if (userTaskResult == null) return HttpNotFound();

            return View(userTaskResult);
        }

        // POST: UserTaskResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _userTaskResultsRepository.Delete(id);
            _userTaskResultsRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
