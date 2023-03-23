using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Data;
using net.core.data.DataAccessFolder;

namespace LanguageWidget.Business.DataAccessFolder.DatabaseEngines.Base
{
    public abstract class BaseDatabase
    {
        private IConfiguration _configuration;
        private ILogger _logger;
        protected string _connectionString;
        public BaseDatabase(IConfiguration configuration, ILogger logger, DatabaseEngine dbEngine, string Env)
        {
            _configuration = configuration;
            _connectionString = configuration.GetSection($"ConnectionStrings:{Enum.GetName(typeof(DatabaseEngine), dbEngine)}:{Env}").Value;
            _logger = logger;
        }

        protected abstract DataTable PrepareDataTable(string Query, List<SQLParameterItem> Params = default);

        protected abstract DbConnection GetConnection();

        #region --- SQL Query Log ---
        public void SqlLog(string Query)
        {
            string sqlQueryLog = _configuration.GetSection("sqlQueryLog").Value;
            if ((sqlQueryLog == "true" || sqlQueryLog == "1") && !string.IsNullOrEmpty(Query))
            {
                _logger.LogInformation(Query + Environment.NewLine + "--- " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ---" + Environment.NewLine);
            }
        }
        #endregion
    }
}
