using Microsoft.Extensions.Configuration;
using net.core.business.DataAccessFolder;

namespace net.core.business.Services.Base;

public class BaseService
{
    protected IConn _conn;
    public BaseService(IConn conn)
    {
        _conn = conn;
    }

    protected T GetSPObject<T>()
    {
        return (T)Activator.CreateInstance(typeof(T), new object[] { _conn });
    }
}