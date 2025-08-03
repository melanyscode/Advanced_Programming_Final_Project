using FinalProject.Data;
using FinalProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryUserTasks : IRepositoryBase<UserTask>
    {

    }
    public class RepositoryUserTasks : RepositoryBase<UserTask>, IRepositoryUserTasks
    {
        public RepositoryUserTasks() : base() { }
    }
}
