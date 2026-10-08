using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagerApp.Tests
{
    // Тесты логики категорий (класс TaskManager)
    [TestClass]
    public class CategoryTests
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
        public void Constructor_HasOnlyDefaultCategory()
        {
            // Act
            var manager = new TaskManager();

            // Assert
            Assert.AreEqual(1, manager.Categories.Count);
            Assert.AreEqual("Без категории", manager.Categories[0]);
        }

        [TestMethod]
        public void AddCategory_ValidName_AddsCategory()
        {
            // Arrange
            var manager = new TaskManager();

            // Act
            manager.AddCategory("Работа");

            // Assert
            Assert.IsTrue(manager.Categories.Contains("Работа"));
        }

        [TestMethod]
        public void AddCategory_NameWithSpaces_IsTrimmed()
        {
            // Arrange
            var manager = new TaskManager();

            // Act
            manager.AddCategory("  Работа  ");

            // Assert
            Assert.IsTrue(manager.Categories.Contains("Работа"));
        }

        [TestMethod]
        public void AddCategory_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddCategory("   "));
        }

        [TestMethod]
        public void AddCategory_DuplicateName_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddCategory("Работа");

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddCategory("Работа"));
        }

        [TestMethod]
        public void AddCategory_DuplicateNameOtherCase_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddCategory("Работа");

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddCategory("РАБОТА"));
        }

        [TestMethod]
        public void AddCategory_NameWithPipe_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddCategory("Работа|Дом"));
        }

        [TestMethod]
        public void AddTask_WithoutCategory_UsesDefaultCategory()
        {
            // Arrange
            var manager = new TaskManager();

            // Act
            manager.AddTask("Купить хлеб");

            // Assert
            Assert.AreEqual("Без категории", manager.Tasks[0].Category);
        }

        [TestMethod]
        public void AddTask_WithCategory_SetsCategory()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddCategory("Работа");

            // Act
            manager.AddTask("Сделать отчёт", "Работа");

            // Assert
            Assert.AreEqual("Работа", manager.Tasks[0].Category);
        }

        [TestMethod]
        public void AddTask_UnknownCategory_ThrowsArgumentException()
        {
            // Arrange
            var manager = new TaskManager();

            // Act & Assert
            Assert.ThrowsExactly<ArgumentException>(() => manager.AddTask("Сделать отчёт", "Работа"));
        }

        [TestMethod]
        public void GetTasksByCategory_ReturnsOnlyTasksOfThatCategory()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddCategory("Работа");
            manager.AddTask("Сделать отчёт", "Работа");
            manager.AddTask("Купить хлеб");

            // Act
            var result = manager.GetTasksByCategory("Работа");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Сделать отчёт", result[0].Description);
        }

        [TestMethod]
        public void SaveTasks_WritesCategoriesAndTasks()
        {
            // Arrange
            var manager = new TaskManager();

            // Act
            manager.AddCategory("Работа");
            manager.AddTask("Сделать отчёт", "Работа");

            // Assert
            var lines = File.ReadAllLines(FileName);
            CollectionAssert.Contains(lines, "CATEGORY|Работа");
            CollectionAssert.Contains(lines, "False|Работа|Сделать отчёт");
        }

        [TestMethod]
        public void LoadTasks_CategoryWithoutTasks_RestoredAfterRestart()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddCategory("Учёба");

            // Act (имитация перезапуска программы)
            var newManager = new TaskManager();

            // Assert
            Assert.IsTrue(newManager.Categories.Contains("Учёба"));
        }

        [TestMethod]
        public void LoadTasks_TaskCategory_RestoredAfterRestart()
        {
            // Arrange
            var manager = new TaskManager();
            manager.AddCategory("Дом");
            manager.AddTask("Помыть посуду", "Дом");

            // Act (имитация перезапуска программы)
            var newManager = new TaskManager();

            // Assert
            Assert.AreEqual(1, newManager.Tasks.Count);
            Assert.AreEqual("Дом", newManager.Tasks[0].Category);
        }
    }

    // Тесты категорий на форме
    [TestClass]
    public class CategoryUITests
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
        public void AddCategoryButton_Click_AddsCategoryToBothLists()
        {
            // Arrange
            _form.newCategoryTextBox.Text = "Работа";

            // Act
            _form.addCategoryButton_Click(null!, EventArgs.Empty);

            // Assert
            Assert.IsTrue(_form.categoryComboBox.Items.Contains("Работа"));
            Assert.IsTrue(_form.filterComboBox.Items.Contains("Работа"));
            Assert.AreEqual("Работа", _form.categoryComboBox.SelectedItem);
            Assert.AreEqual("", _form.newCategoryTextBox.Text);
        }

        [TestMethod]
        public void AddTaskButton_Click_WithCategory_ShowsCategoryInList()
        {
            // Arrange
            _form.newCategoryTextBox.Text = "Работа";
            _form.addCategoryButton_Click(null!, EventArgs.Empty);
            _form.descriptionTextBox.Text = "Сделать отчёт";

            // Act
            _form.addTaskButton_Click(null!, EventArgs.Empty);

            // Assert
            Assert.AreEqual("[ ] Сделать отчёт (Работа)", _form.tasksListBox.Items[0]);
        }

        [TestMethod]
        public void FilterComboBox_SelectCategory_ShowsOnlyThatCategory()
        {
            // Arrange
            _form.newCategoryTextBox.Text = "Работа";
            _form.addCategoryButton_Click(null!, EventArgs.Empty);
            _form.descriptionTextBox.Text = "Сделать отчёт";
            _form.addTaskButton_Click(null!, EventArgs.Empty);
            _form.categoryComboBox.SelectedItem = "Без категории";
            _form.descriptionTextBox.Text = "Купить хлеб";
            _form.addTaskButton_Click(null!, EventArgs.Empty);

            // Act
            _form.filterComboBox.SelectedItem = "Работа";

            // Assert
            Assert.AreEqual(1, _form.tasksListBox.Items.Count);
            Assert.AreEqual("[ ] Сделать отчёт (Работа)", _form.tasksListBox.Items[0]);
        }
    }
}