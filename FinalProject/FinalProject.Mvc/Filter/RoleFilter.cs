using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvancedProgramming.Mvc.Filters
{
    public class RoleFilter : ActionFilterAttribute
    {
        private readonly int _roleId;
        public RoleFilter(int roleId)
        {
            _roleId = roleId;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session["RoleId"] == null)
            {
             
                filterContext.Result = new RedirectResult("/Login/Login");
                return;
            }

            int userRoleId = (int)session["RoleId"];

            if (userRoleId != _roleId)
            {
               
                filterContext.Result = new HttpStatusCodeResult(403, "Access Denied");
            }
        }
    }
}