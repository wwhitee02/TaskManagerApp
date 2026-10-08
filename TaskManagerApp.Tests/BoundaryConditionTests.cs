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

        // Исправление дефекта TC-003: описание из одних пробелов не принимается
        [TestMethod]
        public void AddTask_WhitespaceDescription_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddTask("   "));
        }

        [TestMethod]
        public void EditTask_WhitespaceDescription_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Купить хлеб");

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.EditTask(0, "   "));
        }

        // Исправление дефекта TC-015: задача с символом | сохраняется после перезапуска
        [TestMethod]
        public void LoadTasks_DescriptionWithPipe_TaskRestored()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddTask("Хлеб|молоко");

            // Act (имитация перезапуска программы)
            var newManager = new TaskManager();

            // Assert
            Assert.AreEqual(1, newManager.Tasks.Count);
            Assert.AreEqual("Хлеб|молоко", newManager.Tasks[0].Description);
        }

        // Исправление дефекта TC-016: некорректная строка в файле пропускается
        [TestMethod]
        public void LoadTasks_InvalidLine_IsSkipped()
        {
            // Arrange
            File.WriteAllLines(FileName, new[] { "abc|Задача", "True|Купить хлеб" });

            // Act
            var manager = new TaskManager();

            // Assert
            Assert.AreEqual(1, manager.Tasks.Count);
            Assert.AreEqual("Купить хлеб", manager.Tasks[0].Description);
            Assert.IsTrue(manager.Tasks[0].IsCompleted);
        }
    }
}
