using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class BoundaryConditionTests
    {
        private const string FileName = "tasks.txt";

        [TestInitialize]
        public void SetUp()
        {
            File.Delete(FileName);
        }

        [TestCleanup]
        public void TearDown()
        {
            File.Delete(FileName);
        }

        [TestMethod]
        public void RemoveTask_NegativeIndex_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert
            Assert.ThrowsExactly<IndexOutOfRangeException>(() => manager.RemoveTask(-1));
        }

        [TestMethod]
        public void RemoveTask_IndexEqualsCount_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert (в списке 1 задача, индекс 1 уже за границей)
            Assert.ThrowsExactly<IndexOutOfRangeException>(() => manager.RemoveTask(1));
        }

        [TestMethod]
        public void RemoveTask_EmptyList_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<IndexOutOfRangeException>(() => manager.RemoveTask(0));
        }

        [TestMethod]
        public void ToggleTaskCompletion_NegativeIndex_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert
            Assert.ThrowsExactly<IndexOutOfRangeException>(() => manager.ToggleTaskCompletion(-1));
        }

        [TestMethod]
        public void ToggleTaskCompletion_IndexEqualsCount_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert
            Assert.ThrowsExactly<IndexOutOfRangeException>(() => manager.ToggleTaskCompletion(1));
        }

        [TestMethod]
        public void EditTask_IndexEqualsCount_ThrowsIndexOutOfRangeException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert
            Assert.ThrowsExactly<IndexOutOfRangeException>(() => manager.EditTask(1, "Купить молоко"));
        }

        [TestMethod]
        public void AddTask_OneCharDescription_AddsTask()
        {
            // Arrange
            var manager = new TaskManager();

            // Act (самое короткое допустимое описание)
            manager.AddTask("А");

            // Assert
            Assert.AreEqual(1, manager.Tasks.Count);
        }

        [TestMethod]
        public void AddTask_WhitespaceDescription_IsCurrentlyAccepted()
        {
            // Тест фиксирует текущее поведение: описание из одних пробелов
            // принимается. Это недочёт, он будет исправлен в лабораторной работе 4.
            var manager = new TaskManager();

            // Act
            manager.AddTask("   ");

            // Assert
            Assert.AreEqual(1, manager.Tasks.Count);
        }
    }
}
