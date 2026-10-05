using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManagerApp
{
    public class TaskManager
    {
        private const string FileName = "tasks.txt";

        public List<TaskItem> Tasks { get; private set; }

        public TaskManager()
        {
            Tasks = new List<TaskItem>();
            LoadTasks();
        }

        public void AddTask(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Описание задачи не может быть пустым.");
            }
            Tasks.Add(new TaskItem(description));
            SaveTasks();
        }

        public void EditTask(int index, string newDescription)
        {
            CheckIndex(index);
            if (string.IsNullOrEmpty(newDescription))
            {
                throw new ArgumentException("Описание задачи не может быть пустым.");
            }
            Tasks[index].Description = newDescription;
            SaveTasks();
        }

        public void RemoveTask(int index)
        {
            CheckIndex(index);
            Tasks.RemoveAt(index);
            SaveTasks();
        }

        public void ToggleTaskCompletion(int index)
        {
            CheckIndex(index);
            Tasks[index].IsCompleted = !Tasks[index].IsCompleted;
            SaveTasks();
        }

        private void CheckIndex(int index)
        {
            if (index < 0 || index >= Tasks.Count)
            {
                throw new IndexOutOfRangeException("Некорректный индекс задачи.");
            }
        }

        private void SaveTasks()
        {
            File.WriteAllLines(FileName,
                Tasks.Select(t => $"{t.IsCompleted}|{t.Description}"));
        }

        private void LoadTasks()
        {
            if (File.Exists(FileName))
            {
                var lines = File.ReadAllLines(FileName);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        bool isCompleted = bool.Parse(parts[0]);
                        string description = parts[1];
                        Tasks.Add(new TaskItem(description) { IsCompleted = isCompleted });
                    }
                }
            }
        }
    }
}