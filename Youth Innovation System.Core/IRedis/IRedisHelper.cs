namespace Youth_Innovation_System.Core.IRedis
{
    public interface IRedisHelper
    {
        Task SetValueAsync(string key, string value);
        Task<string> GetValueAsync(string key);
        Task RemoveValueAsync(string key);
    }
}
