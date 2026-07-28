namespace TaskManagerAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public List<Project> Projects { get; set; }= new List<Project>();
    }
}
