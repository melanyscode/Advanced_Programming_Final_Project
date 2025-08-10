using FinalProject.Data;
using FinalProject.Repository;
using Repository;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinalProject.Worker
{
    public class Processor
    {
        private readonly RepositoryUserTask repository = new RepositoryUserTask();

        private bool ApplyRules(UserTask ut, out bool doNow)
        {
            doNow = false;
            if (ut == null)
                return false;
            doNow = ut.ExecutionDate <= DateTime.Now;
            return true;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
   
                    var userTask = repository.GetAll()
                        .Where(ut => ut.Status == "Pending")
                        .OrderBy(ut => ut.ExecutionDate)
                        .FirstOrDefault();

                    var isValid = ApplyRules(userTask, out bool doNow);
                    if (isValid && doNow)
                    {
                        userTask.Status = "Running";
                        repository.Update(userTask);
                        repository.Save();

                        try
                        {
                          
                            var taskRepo = new RepositoryTasks();
                            var task = taskRepo.GetById(userTask.TaskId);

                            if (task != null)
                            {
                                var psi = new ProcessStartInfo
                                {
                                    FileName = "powershell.exe",
                                    Arguments = $"-ExecutionPolicy Bypass -File \"C:\\Scripts\\{task.Executable}\"",
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                };

                                var process = Process.Start(psi);
                                string output = process.StandardOutput.ReadToEnd();
                                string error = process.StandardError.ReadToEnd();
                                process.WaitForExit();

                                userTask.Status = process.ExitCode == 0 ? "Completed" : "Failed";
                                userTask.Result = string.IsNullOrWhiteSpace(error) ? output : error;
                            }
                            else
                            {
                                userTask.Status = "Failed";
                                userTask.Result = "No se encontró la definición de la tarea.";
                            }
                        }
                        catch (Exception ex)
                        {
                            userTask.Status = "Failed";
                            userTask.Result = ex.Message;
                        }

                        repository.Update(userTask);
                        repository.Save();
                    }

                    Thread.Sleep(5000);
                }
            });
        }
    }
}
