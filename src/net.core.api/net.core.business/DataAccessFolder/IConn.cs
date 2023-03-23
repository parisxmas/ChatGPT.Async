
using net.core.data.Base;
using net.core.data.DataAccessFolder;

namespace net.core.business.DataAccessFolder;

public interface IConn
{
    T GetDbConnection<T>(DatabaseEngine dbEngine, EnvironmentEnum Env);
}