using Youth_Innovation_System.Core.Entities.Chat;

namespace Youth_Innovation_System.Core.IServices.IChat
{
    public interface IChatService
    {
        Task<IReadOnlyList<Message>> GetChatHistoryAsync(string senderId, string receiverId);
        Task<bool> DeleteMessageAsync(int messageId, string userId);
    }
}
