namespace TaskManagerAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DeadLine { get; set; }

        public User? User { get; set; }
        public int UserId { get; set; }

        public List<TaskItem> TaskItems { get; set; }= new();


    }
}
