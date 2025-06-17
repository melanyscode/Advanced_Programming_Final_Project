using FinalProject.Data;
using FinalProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryTasks : IRepositoryBase<Tasks>
    {

    }
    public class RepositoryTasks : RepositoryBase<Tasks>, IRepositoryTasks
    {
        public RepositoryTasks() : base() { }
    }
}
