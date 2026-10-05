using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class TaskItemTests
    {
        [TestMethod]
        public void Constructor_SetsDescription()
        {
            // Arrange & Act
            var task = new TaskItem("Купить хлеб");

            // Assert
            Assert.AreEqual("Купить хлеб", task.Description);
        }

        [TestMethod]
        public void Constructor_IsCompletedIsFalseByDefault()
        {
            // Arrange & Act
            var task = new TaskItem("Купить хлеб");

            // Assert
            Assert.IsFalse(task.IsCompleted);
        }

        [TestMethod]
        public void IsCompleted_CanBeSetToTrue()
        {
            // Arrange
            var task = new TaskItem("Купить хлеб");

            // Act
            task.IsCompleted = true;

            // Assert
            Assert.IsTrue(task.IsCompleted);
        }
    }
}