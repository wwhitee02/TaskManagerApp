using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManagerApp
{
    public class TaskManager
    {
        public const string DefaultCategory = "Без категории";
        private const string FileName = "tasks.txt";
        private const string CategoryMarker = "CATEGORY";

        public List<TaskItem> Tasks { get; private set; }
        public List<string> Categories { get; private set; }

        public TaskManager()
        {
            Tasks = new List<TaskItem>();
            Categories = new List<string> { DefaultCategory };
            LoadTasks();
        }

        public void AddCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название категории не может быть пустым.");
            }
            name = name.Trim();
            if (name.Contains('|'))
            {
                throw new ArgumentException("Название категории не может содержать символ «|».");
            }
            if (Categories.Any(c => string.Equals(c, name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("Категория с таким названием уже существует.");
            }
            Categories.Add(name);
            SaveTasks();
        }

        public void AddTask(string description, string? category = null)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Описание задачи не может быть пустым.");
            }
            string taskCategory = string.IsNullOrWhiteSpace(category) ? DefaultCategory : category;
            if (!Categories.Contains(taskCategory))
            {
                throw new ArgumentException("Категория не найдена.");
            }
            Tasks.Add(new TaskItem(description, taskCategory));
            SaveTasks();
        }

        public List<TaskItem> GetTasksByCategory(string category)
        {
            return Tasks.Where(t => t.Category == category).ToList();
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

        // Формат файла:
        // CATEGORY|Работа                  — созданная категория
        // False|Работа|Сделать отчёт       — задача: статус|категория|описание
        private void SaveTasks()
        {
            var lines = Categories
                .Where(c => c != DefaultCategory)
                .Select(c => $"{CategoryMarker}|{c}")
                .Concat(Tasks.Select(t => $"{t.IsCompleted}|{t.Category}|{t.Description}"));
            File.WriteAllLines(FileName, lines);
        }

        private void LoadTasks()
        {
            if (!File.Exists(FileName))
            {
                return;
            }

            foreach (var line in File.ReadAllLines(FileName))
            {
                var parts = line.Split('|', 3);

                if (parts.Length == 2 && parts[0] == CategoryMarker)
                {
                    // Строка с категорией
                    if (!string.IsNullOrWhiteSpace(parts[1]) && !Categories.Contains(parts[1]))
                    {
                        Categories.Add(parts[1]);
                    }
                }
                else if (parts.Length >= 2 && bool.TryParse(parts[0], out bool isCompleted))
                {
                    // Строка с задачей. Старый формат статус|описание тоже читается
                    string category = parts.Length == 3 ? parts[1] : DefaultCategory;
                    string description = parts.Length == 3 ? parts[2] : parts[1];
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        category = DefaultCategory;
                    }
                    if (!Categories.Contains(category))
                    {
                        Categories.Add(category);
                    }
                    Tasks.Add(new TaskItem(description, category) { IsCompleted = isCompleted });
                }
                // Остальные строки некорректны и пропускаются
            }
        }
    }
}