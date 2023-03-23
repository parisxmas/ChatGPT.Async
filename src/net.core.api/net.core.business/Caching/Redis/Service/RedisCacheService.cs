using net.core.business.Caching.Base;
using net.core.business.Caching.Redis.Server;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace net.core.business.Caching.Redis.Service
{
    public class RedisCacheService : ICacheService
    {
        private RedisServer _redisServer;

        public RedisCacheService(RedisServer redisServer)
        {
            _redisServer = redisServer;
        }

        public void Set<T>(string cacheKey, T value)
        {
            //eğer cache yok ise proje hata vermez, timeouta düşen redis exception handle edilir ve proje çalışmaya devam eder
            try
            {
                _redisServer.Database.StringSet(cacheKey, JsonConvert.SerializeObject(value));
            }
            catch (RedisTimeoutException)
            {
            }
        }

        public bool Any(string key)
        {
            return _redisServer.Database.KeyExists(key);
        }

        public string Get(string key)
        {
            //eğer cache yok ise proje hata vermez, timeouta düşen redis exception handle edilir ve proje çalışmaya devam eder
            try
            {
                if (Any(key))
                {
                    var data = _redisServer.Database.StringGet(key);

                    return data;
                }

                return null;
            }
            catch (RedisTimeoutException)
            {
                return null;
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                var data = Get(key);

                return String.IsNullOrEmpty(data) ? default : JsonConvert.DeserializeObject<T>(data);
            }
            catch (RedisTimeoutException)
            {
                return default;
            }
        }

        public List<T> GetByPattern<T>(string pattern)
        {
            try
            {
                var dataList = new List<T>();
                var keys = GetKeysByPattern(pattern);
                foreach (var key in keys)
                {
                    dataList.Add(JsonConvert.DeserializeObject<T>(_redisServer.Database.StringGet(key)));
                }

                return dataList;
            }
            catch (RedisTimeoutException)
            {
                return default;
            }
        }

        public IEnumerable<RedisKey> GetKeysByPattern(string pattern = "*")
        {
            return _redisServer.GetKeysByPattern(pattern);
        }


        public void Remove(string key)
        {
            _redisServer.Database.KeyDelete(key);
        }

        public void Clear()
        {
            _redisServer.FlushDatabase();
        }

        public List<T> DeleteExpiredCaches<T>(IEnumerable<RedisKey> keyList)
        {
            var vals = new List<T>();
            foreach (var key in keyList)
            {
                var cache = this.Get<T>(key);
                var expirationDate = cache.GetType().GetProperty("ExpirationDate").GetValue(cache, null) as DateTime?;

                if (expirationDate.HasValue && expirationDate.Value.CompareTo(DateTime.Now) <= 0)
                {
                    vals.Add(cache);
                    this.Remove(key);
                }
            }

            return vals;
        }

        public void DeleteKeysByPattern(string pattern = "*")
        {
            var keys = GetKeysByPattern(pattern);

            foreach (var key in keys)
            {
                Remove(key);
            }
        }
    }
}