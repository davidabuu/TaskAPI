namespace TaskAPI.Model
{
    public class UserCreatedTask
    {
        public int UserId { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
