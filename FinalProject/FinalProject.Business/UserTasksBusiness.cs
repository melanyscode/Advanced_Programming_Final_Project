using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Data;

namespace FinalProject.Business
{
    public class UserTasksBusiness
    {
        private readonly RepositoryUserTasks repositoryUserTask;
        public UserTasksBusiness()
        {
            repositoryUserTask = new RepositoryUserTasks();
        }
        public IEnumerable<UserTask> GetUserTasks()
        {
            return repositoryUserTask.GetAll();
        }

        public UserTask GetById(int id)
        {
            return repositoryUserTask.GetById(id);
        }
        public void SaveUserTask(int id, UserTask userTasks)
        {
            if (id <= 0)
                repositoryUserTask.Add(userTasks);
            else
                repositoryUserTask.Update(userTasks);
        }
        public void DeleteUserTask(int id)
        {
            repositoryUserTask.Delete(id);
        }
        public void SaveChanges()
        {
            repositoryUserTask.Save();
        }
        public void Update(UserTask userTaskss)
        {
            repositoryUserTask.Update(userTaskss);
        }
    }
}
