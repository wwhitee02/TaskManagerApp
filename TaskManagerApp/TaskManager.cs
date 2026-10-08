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
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Описание задачи не может быть пустым.");
            }
            Tasks.Add(new TaskItem(description));
            SaveTasks();
        }

        public void EditTask(int index, string newDescription)
        {
            CheckIndex(index);
            if (string.IsNullOrWhiteSpace(newDescription))
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
                    // Делим строку только по первой черте: всё после неё — описание
                    var parts = line.Split('|', 2);

                    // Строки с неверным статусом пропускаем, а не падаем с ошибкой
                    if (parts.Length == 2 && bool.TryParse(parts[0], out bool isCompleted))
                    {
                        Tasks.Add(new TaskItem(parts[1]) { IsCompleted = isCompleted });
                    }
                }
            }
        }
    }
}