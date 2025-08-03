using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Data;

namespace FinalProject.Business
{
    public class UserBusiness
    {
        private readonly RepositoryUser repositoryUser;
        public UserBusiness()
        {
            repositoryUser = new RepositoryUser();
        }
        public IEnumerable<User> GetUser()
        {
            return repositoryUser.GetAll();
        }

        public User GetById(int id)
        {
            return repositoryUser.GetById(id);
        }
        public void SaveUser(int id, User user)
        {
            if (id <= 0)
                repositoryUser.Add(user);
            else
                repositoryUser.Update(user);
        }
        public void DeleteUser(int id)
        {
            repositoryUser.Delete(id);
        }
        public void SaveChanges()
        {
            repositoryUser.Save();
        }
        public void Update(User user)
        {
            repositoryUser.Update(user);
        }
    }
}
