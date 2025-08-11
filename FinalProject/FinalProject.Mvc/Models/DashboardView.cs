using FinalProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Mvc.Models
{
    public class DashboardView
    {
        public List<Tasks> AllTasks { get; set; }
        public List<UserTaskView> UserTasks { get; set; }
    }

    public class UserTaskView
    {
        public int UserTaskId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string Result { get; set; }
    }

}