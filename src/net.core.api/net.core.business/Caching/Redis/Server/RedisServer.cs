using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace net.core.business.Caching.Redis.Server
{
    public class RedisServer
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private IServer _server;
        private string configurationString;
        private int _currentDatabaseId = 15;

        public RedisServer(IConfiguration configuration)
        {
            configurationString = configuration.GetSection("RedisConfiguration:Uri").Value;
            ConfigurationOptions option = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { configurationString }
            };
            _connectionMultiplexer = ConnectionMultiplexer.Connect(option);
            _database = _connectionMultiplexer.GetDatabase(_currentDatabaseId);
            _server = _connectionMultiplexer.GetServer(configurationString);
        }

        public IDatabase Database => _database;

        public void FlushDatabase()
        {
            _connectionMultiplexer.GetServer(configurationString).FlushDatabase(_currentDatabaseId);
        }

        public IEnumerable<RedisKey> GetKeysByPattern(string pattern)
        {
            return _server.Keys(database: _currentDatabaseId, pattern: pattern);
        }
    }
}
