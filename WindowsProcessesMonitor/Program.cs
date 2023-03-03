// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Management;
using WindowsProcessesMonitor;


var process = new WindowsProcessesMonitoring();
process.ProcessesMonitor("Notepad", 1, 1);



