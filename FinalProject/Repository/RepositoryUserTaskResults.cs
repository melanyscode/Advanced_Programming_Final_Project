using FinalProject.Data;
using FinalProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryUserTaskResults : IRepositoryBase<UserTaskResults>
    {

    }
    public class RepositoryUserTaskResults : RepositoryBase<UserTaskResults>, IRepositoryUserTaskResults
    {
        public RepositoryUserTaskResults() : base() { }


    }
}
