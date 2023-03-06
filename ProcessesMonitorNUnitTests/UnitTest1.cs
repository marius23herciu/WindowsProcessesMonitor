using WindowsProcessesMonitor;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using System.Threading;
using Moq;
using System.Runtime.CompilerServices;

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
            process = new Process();
            process.StartInfo.FileName = "notepad.exe";
            process.Start();
            Thread.Sleep(1000);
        }

        [Test]
        public void TestProcessMonitor()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetime = 1;
            int monitoringFrequency = 1;

            // Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));
        }
        [TestCase("notepad", 5)]
        public void ProcessesMonitorTestToCheckIfNumberOfInstancesOpenAreClosedAfterMaxLifetime(string processName, int noOfNotepadsToOpen)
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
        [TestCase("notepad", 5)]
        public void ProcessesMonitorTestToCheckIfNumberOfInstancesOpenAreStillActiveIfMaxLifetimeNotReached(string processName, int noOfNotepadsToOpen)
        {
            // Arrange
            startNotepad.Start(noOfNotepadsToOpen);
            int maxLifetime = 5;
            int monitoringFrequency = 1;

            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();

            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(noOfNotepadsToOpen));
        }
        [TestCase("notepad", 3)]
        public void ProcessesMonitorTestToCheckIfAfterOpenMoreInstancesOfAProcessAtTheEndAreActiveOnlyThoseWithinLifeSpan(string processName, int noOfNotepadsToOpen)
        {
            // Arrange
            startNotepad.Start(noOfNotepadsToOpen);
            int maxLifetime = 2;
            int monitoringFrequency = 1;

            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            startNotepad.Start(noOfNotepadsToOpen);
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));

            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(noOfNotepadsToOpen));
        }
        [TestCase("notepad", 3, 5)]
        public void ProcessesMonitorTestToCheckIfAfterOpenMoreInstancesOfAProcessAtTheEndAreActiveOnlyThoseWithinLifeSpan2(string processName, int noOfNotepadsToOpen, int noOfMonitoringFrequencyCycles)
        {
            // Arrange
            startNotepad.Start(noOfNotepadsToOpen);
            int maxLifetime = 2;
            int monitoringFrequency = 1;

            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            for (int i = 0; i < noOfMonitoringFrequencyCycles; i++)
            {
                startNotepad.Start(noOfNotepadsToOpen);
                Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));
            }

            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(noOfNotepadsToOpen));
        }
        [TestCase("notepad")]
        public void ProcessesMonitorTestToCheckIfNumberOfInstancessIs0IfNeverAcitvateAProcess(string processName)
        {
            // Arrange
            startNotepad.Start(0);
            int maxLifetime = 5;
            int monitoringFrequency = 1;

            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));

            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(0));
        }
        [TestCase("notepad")]
        public void ProcessesMonitorTestToCheckIfNumberOfInstancessIs0AfterStartingWithExceedingLifespanProcesses(string processName)
        {
            // Arrange
            startNotepad.Start(10);
            int maxLifetime = 1;
            int monitoringFrequency = 1;
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));

            //Act
            var thread = new Thread(() => windowsProcessesMonitoring.ProcessesMonitor(processName, maxLifetime, monitoringFrequency));
            thread.Start();
            Thread.Sleep(TimeSpan.FromMinutes(monitoringFrequency));

            var notepad = System.Diagnostics.Process.GetProcessesByName(processName);

            //Assert
            Assert.That(notepad.Count, Is.EqualTo(0));
        }
    }
}