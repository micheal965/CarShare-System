using StackExchange.Redis;
using Youth_Innovation_System.Core.IRedis;
namespace Youth_Innovation_System.Repository.Redis
{
    public class RedisHelper : IRedisHelper
    {
        private readonly IDatabase db;
        public RedisHelper(IConnectionMultiplexer connectionMultiplexer)
        {
            db = connectionMultiplexer.GetDatabase();
        }
        public async Task SetValueAsync(string key, string value) => await db.StringSetAsync(key, value);
        public async Task<string> GetValueAsync(string key) => await db.StringGetAsync(key);
        public async Task RemoveValueAsync(string key) => await db.KeyDeleteAsync(key);
    }
}
