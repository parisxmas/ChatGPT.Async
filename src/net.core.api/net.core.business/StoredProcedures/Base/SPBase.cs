using net.core.business.DataAccessFolder;
using net.core.business.DataAccessFolder.DatabaseEngines;
using net.core.data.Base;
using net.core.data.DataAccessFolder;

namespace net.core.business.StoredProcedures.Base;

public class SPBase:IDisposable
{
    public DatabaseMSSQL _sqlEngine;
    public SPBase(IConn conn)
    {
        _sqlEngine = conn.GetDbConnection<DatabaseMSSQL>(DatabaseEngine.MSSQL, EnvironmentEnum.Dev);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}