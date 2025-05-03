using Youth_Innovation_System.Core.Entities.Chat;

namespace Youth_Innovation_System.Core.Specifications.ChatSpecifications
{
    public class DeleteMessageSpecifications : BaseSpecification<Message>
    {
        public DeleteMessageSpecifications(int messageId, string userId)
            : base(m => m.Id == messageId && m.SenderId == userId)
        {

        }
    }
}
