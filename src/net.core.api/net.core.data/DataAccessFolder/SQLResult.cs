using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.core.data.DataAccessFolder
{
    public class SQLResult
    {
        public string Message { get; set; }
        public string SqlQuery { get; set; }
        public DataTable Table { get; set; }

        public override string ToString()
        {
            return Message;
        }

        public SQLResult() { }
        public SQLResult(string Message, string SqlQuery, DataTable Table)
        {
            this.Message = Message;
            this.SqlQuery = SqlQuery;
            this.Table = Table;
        }
    }
}
