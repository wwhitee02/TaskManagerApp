namespace TaskManagerApp
{
    public class TaskItem
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; }

        public TaskItem(string description, string category = TaskManager.DefaultCategory)
        {
            Description = description;
            IsCompleted = false;
            Category = category;
        }
    }
}