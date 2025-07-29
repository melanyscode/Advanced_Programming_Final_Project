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
        private readonly RepositoryTasks repository = new RepositoryTasks();

        private bool ApplyRules(Tasks t, out bool doNow) 
        {
            bool isNull = t == null;
            doNow = t.ExecutionDate <= DateTime.Now;
            bool isValidDate = false;
            if (!doNow)
                isValidDate = t.ExecutionDate >= DateTime.Now;
            return isNull; 
        }

        public void Start()
        {
            Task.Run(() =>
            {
            // T1 not null && friday 7pm (Monday)
            // T1 not null && friday 7pm TRUE (FRIDAY)
                while (true)
                {
                    var task = repository.GetAll()
                        .Where(t => t.Status == "Pending")
                        .OrderByDescending(t => t.Priority)
                        .ThenBy(t => t.ExecutionDate)
                        .FirstOrDefault();

                    var isValid = ApplyRules(task, out bool doNow);
                    if (isValid && doNow)
                    {
                        task.Status = "Running";
                        task.UpdatedAt = DateTime.Now;
                        repository.Update(task);
                        repository.Save();

                        try
                        {
                            var psi = new ProcessStartInfo
                            {
                                FileName = "powershell.exe",
                                Arguments = $"-ExecutionPolicy Bypass -File \"C:\\Scripts\\{task.SimulatedCommand}\"",
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };

                            var process = Process.Start(psi);
                            string output = process.StandardOutput.ReadToEnd();
                            string error = process.StandardError.ReadToEnd();
                            process.WaitForExit();

                            task.Status = process.ExitCode == 0 ? "Completed" : "Failed";
                            task.Result = string.IsNullOrWhiteSpace(error) ? output : error;
                        }
                        catch (Exception ex)
                        {
                            task.Status = "Failed";
                            task.Result = ex.Message;
                        }

                        task.UpdatedAt = DateTime.Now;
                        repository.Update(task);
                        repository.Save();
                    }

                    Thread.Sleep(5000); 
                }
            });
        }
    }
}
