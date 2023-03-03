using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProcessesMonitorNUnitTests
{
    public class StartNotepad
    {
        public void Start (int noOfNotepadsToOpen)
        {
            var processesToClose = System.Diagnostics.Process.GetProcessesByName("notepad");
            foreach (var processs in processesToClose)
            {
                processs.Kill();
            }
            for (int i = 0; i < noOfNotepadsToOpen; i++)
            {
                Process.Start(new ProcessStartInfo { FileName = @"C:\Users\seb\Desktop\WindowsProcessesMonitor\WindowsProcessesMonitor\bin\Debug\net6.0\test.txt", UseShellExecute = true });
            }
        }
    }
}
