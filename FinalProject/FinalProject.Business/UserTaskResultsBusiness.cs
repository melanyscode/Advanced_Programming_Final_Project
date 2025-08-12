using Repository;
using System.Collections.Generic;
using FinalProject.Data;

namespace FinalProject.Business
{
    public class UserTaskResultsBusiness
    {
        private readonly RepositoryUserTaskResults repositoryUserTaskResults;

        public UserTaskResultsBusiness()
        {
            repositoryUserTaskResults = new RepositoryUserTaskResults();
        }

        public IEnumerable<UserTaskResults> GetUserTaskResults()
        {
            return repositoryUserTaskResults.GetAll();
        }

        public UserTaskResults GetById(int id)
        {
            return repositoryUserTaskResults.GetById(id);
        }

        public void SaveUserTaskResult(int id, UserTaskResults userTaskResult)
        {
            if (id <= 0)
                repositoryUserTaskResults.Add(userTaskResult);
            else
                repositoryUserTaskResults.Update(userTaskResult);
        }

        public void DeleteUserTaskResult(int id)
        {
            repositoryUserTaskResults.Delete(id);
        }

        public void SaveChanges()
        {
            repositoryUserTaskResults.Save();
        }

        public void Update(UserTaskResults userTaskResult)
        {
            repositoryUserTaskResults.Update(userTaskResult);
        }
    }
}
