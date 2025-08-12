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
        private readonly RepositoryUserTasks repository = new RepositoryUserTasks();

        private bool ApplyRules(UserTask ut, out bool doNow)
        {
            doNow = false;
            if (ut == null) return false;

            if (!ut.LastExecution.HasValue)
            {
                if (ut.ExecutionTime.HasValue && ut.RepeatIntervalHours == 24)
                {
                    var todayExec = DateTime.Today.Add(ut.ExecutionTime.Value);
                    doNow = DateTime.Now >= todayExec;
                }
                else
                {
                    doNow = ut.ExecutionDate <= DateTime.Now;
                }
                return true;
            }
            if (ut.ExecutionTime.HasValue && ut.RepeatIntervalHours == 24)
            {
                var nextExec = ut.LastExecution.Value.Date
                    .AddDays(1)
                    .Add(ut.ExecutionTime.Value);
                doNow = DateTime.Now >= nextExec;
                return true;
            }
            if (ut.RepeatIntervalHours.HasValue && ut.RepeatIntervalHours > 0)
            {
                var nextExec = ut.LastExecution.Value.AddMinutes(ut.RepeatIntervalHours.Value);
                doNow = DateTime.Now >= nextExec;
                return true;
            }
            doNow = false;
            return false;
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var userTask = repository.GetAll()
                        .Where(ut => ut.Status == "Pending" || ut.Status == "Completed")
                        .OrderBy(ut => ut.ExecutionDate).ToList();
                    foreach(var ut in userTask)
                    {
                        var isValid = ApplyRules(ut, out bool doNow);

                        if (isValid && doNow || ut.Status == "Pending")
                        {
                            ut.Status = "Running";
                            repository.Update(ut);
                            repository.Save();

                            try
                            {
                                var taskRepo = new RepositoryTasks();
                                var task = taskRepo.GetById(ut.TaskId);
                                var userTaskResultsRepo = new RepositoryUserTaskResults();
                                var resultHistory = new UserTaskResults();

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

                                    ut.Status = process.ExitCode == 0 ? "Completed" : "Failed";
                                    ut.Result = string.IsNullOrWhiteSpace(error) ? output : error;
                                    ut.LastExecution = DateTime.Now;

                                    repository.Update(ut);
                                    repository.Save();
                                    try
                                    {
                                        resultHistory = new UserTaskResults
                                        {
                                            UserTaskId = ut.UserTaskId,
                                            ExecutionDate = ut.LastExecution.Value,
                                            ResultValue = ut.Result
                                        };
                                        userTaskResultsRepo.Add(resultHistory);
                                        userTaskResultsRepo.Save();
                                    }catch(Exception ex)
                                    {
                                        Debug.WriteLine($"error guardando: {ex.Message}");
                                    }
                                    
                                    Debug.WriteLine($"ejecutando tarea {ut.UserTaskId}");
                                }
                                else
                                {
                                    ut.Status = "Failed";
                                    ut.Result = "Tasks doesn't exist";

                                    repository.Update(ut);
                                    repository.Save();
                                }
                            }
                            catch (Exception ex)
                            {
                                ut.Status = "Failed";
                                ut.Result = ex.Message;

                                repository.Update(ut);
                                repository.Save();
                                Debug.WriteLine($"Error ejecutando tarea {ut.UserTaskId}: {ex}");
                            }

                            repository.Update(ut);
                            repository.Save();
                        }

                    }

                    Thread.Sleep(5000); 
                }
            });
        }
    }
}
