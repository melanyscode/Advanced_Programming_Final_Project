using FinalProject.Data;
using FinalProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryUsers : IRepositoryBase<User>
    {

    }
    public class RepositoryUser : RepositoryBase<User>, IRepositoryUsers
    {
        public RepositoryUser() : base() { }
    }
}