namespace Youth_Innovation_System.Core.Entities.Chat
{
    public class Message : BaseEntity
    {
        public string SenderId { get; set; }//from identitydb
        public string ReceiverId { get; set; }//from identitydb
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
