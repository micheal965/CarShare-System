namespace Youth_Innovation_System.Core.Entities
{
    public class Notification : BaseEntity
    {
        public string userId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
