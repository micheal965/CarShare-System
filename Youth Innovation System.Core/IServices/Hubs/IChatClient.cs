namespace Youth_Innovation_System.Core.IServices.IHubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string userName, string message);
        Task MessageDeleted(int messageId);
    }
}
