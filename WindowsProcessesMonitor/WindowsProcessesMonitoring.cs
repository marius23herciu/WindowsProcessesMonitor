using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsProcessesMonitor
{
    public class WindowsProcessesMonitoring
    {
        public void ProcessesMonitor(string processName, int maxLifetime, int monitoringFrequency)
        {
            // Start monitoring processes
            Console.WriteLine($"Monitoring process {processName} with a maximum lifetime of {maxLifetime} minutes and a monitoring frequency of {monitoringFrequency} minutes.");
            Console.WriteLine("Press Q to end monitoring process");
            while (true)
            {
                // Check if the 'q' key is pressed
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Monitoring stopped.");
                    break;
                }

                // Get the list of running processes with the given name
                var processes = Process.GetProcessesByName(processName);

                // If no process is found, sleep for the monitoring frequency and continue
                if (processes.Length == 0)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));
                    continue;
                }

                // Check if any process has lived longer than the allowed duration
                foreach (Process process in processes)
                {
                    TimeSpan runtime = DateTime.Now - process.StartTime;
                    if (runtime >= TimeSpan.FromMinutes(maxLifetime))
                    {
                        process.Kill();
                        Console.WriteLine($"Process {processName} killed at {DateTime.Now} because it exceeded the maximum lifetime of {maxLifetime} minutes.");
                    }
                }
            }
        }
    }
}
