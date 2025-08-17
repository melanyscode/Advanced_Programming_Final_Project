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
    [RoleFilter("Admin")]
    public class UsersController : Controller
    {
        private readonly UserBusiness userBusiness;

        public UsersController()
        {
            userBusiness = new UserBusiness();
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(userBusiness.GetUser());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = userBusiness.GetById(id.Value);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Username,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                userBusiness.SaveUser(0, user);
                userBusiness.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = userBusiness.GetById(id.Value);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Username,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                userBusiness.SaveUser(user.UserId, user); 
                userBusiness.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            User user = userBusiness.GetById(id.Value);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userBusiness.DeleteUser(id);
            userBusiness.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
