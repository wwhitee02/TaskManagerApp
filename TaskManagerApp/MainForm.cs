using System;
using System.Windows.Forms;

namespace TaskManagerApp
{
    public partial class MainForm : Form
    {
        private TaskManager taskManager;

        public MainForm()
        {
            InitializeComponent();
            taskManager = new TaskManager();
            UpdateTasksList();
        }

        private void UpdateTasksList()
        {
            tasksListBox.Items.Clear();
            foreach (var task in taskManager.Tasks)
            {
                tasksListBox.Items.Add($"{(task.IsCompleted ? "[X]" : "[ ]")} {task.Description}");
            }
        }

        private void addTaskButton_Click(object sender, EventArgs e)
        {
            try
            {
                taskManager.AddTask(descriptionTextBox.Text);
                descriptionTextBox.Clear();
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void editTaskButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для редактирования!");
                return;
            }
            try
            {
                taskManager.EditTask(tasksListBox.SelectedIndex, descriptionTextBox.Text);
                descriptionTextBox.Clear();
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void removeTaskButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для удаления!");
                return;
            }
            try
            {
                taskManager.RemoveTask(tasksListBox.SelectedIndex);
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toggleCompletionButton_Click(object sender, EventArgs e)
        {
            if (tasksListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для изменения статуса!");
                return;
            }
            try
            {
                taskManager.ToggleTaskCompletion(tasksListBox.SelectedIndex);
                UpdateTasksList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}