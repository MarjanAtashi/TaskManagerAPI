namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;
        public DateTime? DeadLine { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }


    }
}
