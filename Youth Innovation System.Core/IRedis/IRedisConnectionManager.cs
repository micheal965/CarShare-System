namespace Youth_Innovation_System.Core.IRedis
{
    public interface IRedisConnectionManager
    {
        Task AddConnectionAsync(string userId, string connectionId);
        Task RemoveConnectionAsync(string userId);
        Task<string> GetConnectionAsync(string userId);
        Task SetUserOnlineAsync(string userId);
        Task SetUserOfflineAsync(string userId);
        Task<bool> IsUserOnlineAsync(string userId);

    }

}
