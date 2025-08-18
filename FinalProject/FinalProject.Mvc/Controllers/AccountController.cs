using FinalProject.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserBusiness userBusiness;
        public AccountController()
        {
            userBusiness = new UserBusiness();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

    
      [HttpPost]
        public ActionResult Login(string username, string password)
        {

            var user = userBusiness.Login(username, password);
            if (user != null)
            {
                Session["UserId"] = user.UserId;
                Session["User"] = user.Username;
                Session["RoleId"] = user.Role;
                return RedirectToAction("Index", "Home");
            }
            
                ViewBag.Error = "Invalid username or password";
                return View("Index");
            

               
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("Index");
        }
    }
    }