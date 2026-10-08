using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TaskManagerApp
{
    public partial class MainForm : Form
    {
        private const string AllCategories = "Все категории";

        internal TaskManager taskManager;

        // Задачи, которые сейчас показаны в списке (с учётом фильтра)
        private List<TaskItem> displayedTasks = new List<TaskItem>();

        public MainForm()
        {
            InitializeComponent();
            taskManager = new TaskManager();
            UpdateCategoryLists();
            UpdateTasksList();
        }

        // Заполняет оба выпадающих списка категориями
        private void UpdateCategoryLists()
        {
            string? selectedCategory = categoryComboBox.SelectedItem as string;
            string? selectedFilter = filterComboBox.SelectedItem as string;

            categoryComboBox.Items.Clear();
            filterComboBox.Items.Clear();
            filterComboBox.Items.Add(AllCategories);
            foreach (var category in taskManager.Categories)
            {
                categoryComboBox.Items.Add(category);
                filterComboBox.Items.Add(category);
            }

            categoryComboBox.SelectedItem = selectedCategory ?? TaskManager.DefaultCategory;
            if (categoryComboBox.SelectedIndex == -1)
            {
                categoryComboBox.SelectedIndex = 0;
            }

            filterComboBox.SelectedItem = selectedFilter ?? AllCategories;
            if (filterComboBox.SelectedIndex == -1)
            {
                filterComboBox.SelectedIndex = 0;
            }
        }

        // Показывает задачи с учётом выбранного фильтра
        private void UpdateTasksList()
        {
            string? filter = filterComboBox.SelectedItem as string;
            displayedTasks = (filter == null || filter == AllCategories)
                ? taskManager.Tasks.ToList()
                : taskManager.GetTasksByCategory(filter);

            tasksListBox.Items.Clear();
            foreach (var task in displayedTasks)
            {
                string text = $"{(task.IsCompleted ? "[X]" : "[ ]")} {task.Description}";
                if (task.Category != TaskManager.DefaultCategory)
                {
                    text += $" ({task.Category})";
                }
                tasksListBox.Items.Add(text);
            }
        }

        // Номер выбранной задачи в общем списке задач или -1, если ничего не выбрано
        private int GetSelectedTaskIndex()
        {
            int i = tasksListBox.SelectedIndex;
            if (i < 0 || i >= displayedTasks.Count)
            {
                return -1;
            }
            return taskManager.Tasks.IndexOf(displayedTasks[i]);
        }

        internal void addTaskButton_Click(object sender, EventArgs e)
        {
            try
            {
                taskManager.AddTask(descriptionTextBox.Text, categoryComboBox.SelectedItem as string);
                descriptionTextBox.Clear();
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void editTaskButton_Click(object sender, EventArgs e)
        {
            int index = GetSelectedTaskIndex();
            if (index == -1)
            {
                MessageBox.Show("Выберите задачу для редактирования!");
                return;
            }
            try
            {
                taskManager.EditTask(index, descriptionTextBox.Text);
                descriptionTextBox.Clear();
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void removeTaskButton_Click(object sender, EventArgs e)
        {
            int index = GetSelectedTaskIndex();
            if (index == -1)
            {
                MessageBox.Show("Выберите задачу для удаления!");
                return;
            }
            try
            {
                taskManager.RemoveTask(index);
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void toggleCompletionButton_Click(object sender, EventArgs e)
        {
            int index = GetSelectedTaskIndex();
            if (index == -1)
            {
                MessageBox.Show("Выберите задачу для изменения статуса!");
                return;
            }
            try
            {
                taskManager.ToggleTaskCompletion(index);
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal void addCategoryButton_Click(object sender, EventArgs e)
        {
            try
            {
                taskManager.AddCategory(newCategoryTextBox.Text);
                string created = newCategoryTextBox.Text.Trim();
                newCategoryTextBox.Clear();
                UpdateCategoryLists();
                categoryComboBox.SelectedItem = created;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void filterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTasksList();
        }
    }
}