using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Data;

namespace FinalProject.Business
{
    public class TasksBusiness
    {
        private readonly RepositoryTasks repositoryTask;
        public TasksBusiness()
        {
            repositoryTask = new RepositoryTasks();
        }
        public IEnumerable<Tasks> GetTasks()
        {
            return repositoryTask.GetAll();
        }

        public Tasks GetById(int id)
        {
            return repositoryTask.GetById(id);
        }
        public void SaveTask(int id, Tasks task)
        {
            if (id <= 0)
                repositoryTask.Add(task);
            else
                repositoryTask.Update(task);
        }
        public void DeleteTask(int id)
        {
            repositoryTask.Delete(id);
        }
        public void SaveChanges()
        {
            repositoryTask.Save();
        }
        public void Update(Tasks tasks)
        {
            repositoryTask.Update(tasks);
        }
    }
}
