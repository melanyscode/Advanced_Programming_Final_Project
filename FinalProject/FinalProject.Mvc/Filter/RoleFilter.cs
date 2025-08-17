using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvancedProgramming.Mvc.Filters
{
    public class RoleFilter : ActionFilterAttribute
    {
                private readonly string _role;
        public RoleFilter(string role)
        {
            _role = role;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session["RoleId"] == null)
            {
                filterContext.Result = new RedirectResult("/Login/Login");
                return;
            }

            string userRole = session["RoleId"].ToString();

            if (!userRole.Equals(_role, StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new HttpStatusCodeResult(403, "Access Denied");
            }
        }
    }
}
