using TaskAPI.Enums;

namespace TaskAPI.Model
{
    public class Notification
    {
        public int? NotificationId { get; set; }
        public string? Message { get; set; }
        public NotificationType? NotificationType { get; set; }
        public int? UserId { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
