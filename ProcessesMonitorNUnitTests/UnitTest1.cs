using WindowsProcessesMonitor;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Threading;

namespace ProcessesMonitorNUnitTests
{
    public class Tests
    {
        private WindowsProcessesMonitoring windowsProcessesMonitoring { get; set; } = null!;
        private StartNotepad startNotepad { get; set; } = null;
        private Process process;
        [SetUp]
        public void Setup()
        {
            windowsProcessesMonitoring = new WindowsProcessesMonitoring();
            startNotepad = new StartNotepad();
            // Start a test process
            process = new Process();
            process.StartInfo.FileName = "notepad.exe";
            process.Start();
            Thread.Sleep(1000); // Wait for the process to start
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    // Kill the test process
        //    if (!process.HasExited)
        //    {
        //        process.Kill();
        //    }
        //}

        [Test]
        public void TestProcessMonitor()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetime = 1;
            int monitoringFrequency = 1;

            // Act
            var thread = new Thread(() =>windowsProcessesMonitoring.ProcessesMonitor( processName, maxLifetime, monitoringFrequency));
            thread.Start();
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));
        }
        [TestCase("notepad", 3)]
        public void ProcessesMonitorTest(string processName, int noOfNotepadsToOpen)
        {
            // Arrange

            startNotepad.Start(noOfNotepadsToOpen);
            int maxLifetime = 1;
            int monitoringFrequency = 1;
            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));

            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(0));
        }
        [TestCase("notepad", 3)]
        public void ProcessesMonitorTest2(string processName, int noOfNotepadsToOpen)
        {
            // Arrange

            startNotepad.Start(noOfNotepadsToOpen);
            int maxLifetime = 1;
            int monitoringFrequency = 1;
            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(noOfNotepadsToOpen));
        }
        
    }
    }