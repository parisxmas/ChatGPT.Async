using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using net.core.data.Base;
using net.core.data.DataAccessFolder;

namespace net.core.business.DataAccessFolder
{
    public class Conn : IConn
    {
        private IConfiguration _configuration;
        private ILogger<LoggerClass> _logger;
        public Conn(IConfiguration configuration, ILogger<LoggerClass> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public T GetDbConnection<T>(DatabaseEngine dbEngine, EnvironmentEnum Env) // T: DbConnection
        {
            Assembly assembly = Assembly.Load(GLOBALS.DatabaseConnectorNamespacePath);

            var className = Enum.GetName(typeof(DatabaseEngine), dbEngine);
            var t = assembly.GetType(GLOBALS.DatabaseConnectorNamespacePath + $".DataAccessFolder.DatabaseEngines.Database{className}");

            return t == default ? default : (T)Activator.CreateInstance(t, new object[] { _configuration, _logger, Env.ToString() });
        }
    }
}
