using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerApp.Tests
{
    [TestClass]
    public class UITests
    {
        private const string FileName = "tasks.txt";
        private MainForm _form = null!;

        [TestInitialize]
        public void SetUp()
        {
            File.Delete(FileName);
            _form = new MainForm();
        }

        [TestCleanup]
        public void TearDown()
        {
            _form.Dispose();
            File.Delete(FileName);
        }

        [TestMethod]
        public void DescriptionTextBox_IsEnabledAndEmpty()
        {
            Assert.IsTrue(_form.descriptionTextBox.Enabled);
            Assert.AreEqual("", _form.descriptionTextBox.Text);
        }

        [TestMethod]
        public void AddTaskButton_IsEnabled()
        {
            Assert.IsTrue(_form.addTaskButton.Enabled);
        }

        [TestMethod]
        public void EditTaskButton_IsEnabled()
        {
            Assert.IsTrue(_form.editTaskButton.Enabled);
        }

        [TestMethod]
        public void RemoveTaskButton_IsEnabled()
        {
            Assert.IsTrue(_form.removeTaskButton.Enabled);
        }

        [TestMethod]
        public void ToggleCompletionButton_IsEnabled()
        {
            Assert.IsTrue(_form.toggleCompletionButton.Enabled);
        }

        [TestMethod]
        public void TasksListBox_IsEnabledAndEmptyOnStart()
        {
            Assert.IsTrue(_form.tasksListBox.Enabled);
            Assert.AreEqual(0, _form.tasksListBox.Items.Count);
        }

        [TestMethod]
        public void AddTaskButton_Click_ValidData_AddsTaskToList()
        {
            // Arrange
            _form.descriptionTextBox.Text = "Купить хлеб";

            // Act
            _form.addTaskButton_Click(null!, EventArgs.Empty);

            // Assert
            Assert.AreEqual(1, _form.taskManager.Tasks.Count);
            Assert.AreEqual("[ ] Купить хлеб", _form.tasksListBox.Items[0]);
            Assert.AreEqual("", _form.descriptionTextBox.Text);
        }

        [TestMethod]
        public void RemoveTaskButton_Click_ItemSelected_RemovesTask()
        {
            // Arrange
            _form.descriptionTextBox.Text = "Купить хлеб";
            _form.addTaskButton_Click(null!, EventArgs.Empty);
            _form.tasksListBox.SelectedIndex = 0;

            // Act
            _form.removeTaskButton_Click(null!, EventArgs.Empty);

            // Assert
            Assert.AreEqual(0, _form.taskManager.Tasks.Count);
            Assert.AreEqual(0, _form.tasksListBox.Items.Count);
        }

        [TestMethod]
        public void ToggleCompletionButton_Click_ItemSelected_MarksTaskCompleted()
        {
            // Arrange
            _form.descriptionTextBox.Text = "Купить хлеб";
            _form.addTaskButton_Click(null!, EventArgs.Empty);
            _form.tasksListBox.SelectedIndex = 0;

            // Act
            _form.toggleCompletionButton_Click(null!, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_form.taskManager.Tasks[0].IsCompleted);
            Assert.AreEqual("[X] Купить хлеб", _form.tasksListBox.Items[0]);
        }

        [TestMethod]
        public void EditTaskButton_Click_ItemSelected_ChangesDescription()
        {
            // Arrange
            _form.descriptionTextBox.Text = "Купить хлеб";
            _form.addTaskButton_Click(null!, EventArgs.Empty);
            _form.tasksListBox.SelectedIndex = 0;
            _form.descriptionTextBox.Text = "Купить молоко";

            // Act
            _form.editTaskButton_Click(null!, EventArgs.Empty);

            // Assert
            Assert.AreEqual("Купить молоко", _form.taskManager.Tasks[0].Description);
            Assert.AreEqual("[ ] Купить молоко", _form.tasksListBox.Items[0]);
        }
    }
}