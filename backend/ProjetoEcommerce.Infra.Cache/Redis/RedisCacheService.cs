using StackExchange.Redis;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.Cache.Redis
{
    public interface IRedisCacheService
    {
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
        Task RemoveByPatternAsync(string pattern);
    }

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (!value.HasValue)
                return default;

            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task RemoveByPatternAsync(string pattern)
        {
            var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints().First());
            var keys = server.Keys(pattern: pattern);

            foreach (var key in keys)
            {
                await _db.KeyDeleteAsync(key);
            }
        }
    }
}
