using Youth_Innovation_System.Core.Entities.Chat;

namespace Youth_Innovation_System.Core.Specifications.ChatSpecifications
{
    public class FetchChatHistorySpecifications : BaseSpecification<Message>
    {
        public FetchChatHistorySpecifications(string SenderId, string ReceiverId)
            : base(m => m.SenderId == SenderId && m.ReceiverId == ReceiverId)
        {
            AddOrderByDesc(m => m.Timestamp);
        }
    }
}
