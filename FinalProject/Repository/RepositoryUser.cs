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
        public User GetByUsernameAndPassword(string username, string password)
        {
            return _set.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}