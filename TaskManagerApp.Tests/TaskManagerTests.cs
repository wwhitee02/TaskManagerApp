using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class TaskManagerTests
    {
        private const string FileName = "tasks.txt";

        // Перед каждым тестом удаляем файл, чтобы начинать с пустого списка
        [TestInitialize]
        public void SetUp()
        {
            File.Delete(FileName);
        }

        // После каждого теста удаляем файл, чтобы не мешать другим тестам
        [TestCleanup]
        public void TearDown()
        {
            File.Delete(FileName);
        }

        [TestMethod]
        public void Constructor_NoFile_EmptyList()
        {
            // Act
            var manager = new TaskManager();

            // Assert
            Assert.AreEqual(0, manager.Tasks.Count);
        }

        [TestMethod]
        public void AddTask_ValidDescription_AddsTask()
        {
            // Arrange
            var manager = new TaskManager();

            // Act
            manager.AddTask("Купить хлеб");

            // Assert
            Assert.AreEqual(1, manager.Tasks.Count);
            Assert.AreEqual("Купить хлеб", manager.Tasks[0].Description);
        }

        [TestMethod]
        public void AddTask_EmptyDescription_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddTask(""));
        }

        [TestMethod]
        public void AddTask_NullDescription_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddTask(null!));
        }

        [TestMethod]
        public void RemoveTask_ValidIndex_RemovesTask()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act
            manager.RemoveTask(0);

            // Assert
            Assert.AreEqual(0, manager.Tasks.Count);
        }

        [TestMethod]
        public void ToggleTaskCompletion_ValidIndex_MarksCompleted()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act
            manager.ToggleTaskCompletion(0);

            // Assert
            Assert.IsTrue(manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        public void ToggleTaskCompletion_Twice_ReturnsToNotCompleted()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act
            manager.ToggleTaskCompletion(0);
            manager.ToggleTaskCompletion(0);

            // Assert
            Assert.IsFalse(manager.Tasks[0].IsCompleted);
        }

        [TestMethod]
        public void EditTask_ValidData_ChangesDescription()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act
            manager.EditTask(0, "Купить молоко");

            // Assert
            Assert.AreEqual("Купить молоко", manager.Tasks[0].Description);
        }

        [TestMethod]
        public void EditTask_EmptyDescription_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.EditTask(0, ""));
        }

        [TestMethod]
        public void SaveTasks_WritesCorrectFormat()
        {
            // Arrange
            var manager = new TaskManager();

            // Act
            manager.AddTask("Купить хлеб");

            // Assert
            var lines = File.ReadAllLines(FileName);
            Assert.AreEqual("False|Купить хлеб", lines[0]);
        }

        [TestMethod]
        public void LoadTasks_AfterRestart_TasksRestored()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");
            manager.ToggleTaskCompletion(0);

            // Act (имитация перезапуска программы)
            var newManager = new TaskManager();

            // Assert
            Assert.AreEqual(1, newManager.Tasks.Count);
            Assert.AreEqual("Купить хлеб", newManager.Tasks[0].Description);
            Assert.IsTrue(newManager.Tasks[0].IsCompleted);
        }
    }
}