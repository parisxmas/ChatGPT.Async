using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using net.core.data.DataAccessFolder;

namespace LanguageWidget.Business.DataAccessFolder.DatabaseEngines.Base
{
    public interface IDatabase
    {
        string RunQuery(string Query, List<SQLParameterItem> Params = null);

        bool ExistRecord(string Query, List<SQLParameterItem> Params = null);

        SQLResult Run(string Query, List<SQLParameterItem> Params = null);
    }

}
