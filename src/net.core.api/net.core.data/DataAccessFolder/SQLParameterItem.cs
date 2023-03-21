using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.core.data.DataAccessFolder
{
    public class SQLParameterItem
    {
        public string Name { get; set; }
        public SqlColumnType Type { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public SQLParameterItem()
        {

        }

        public SQLParameterItem(string SetText, SqlColumnType SetType, object SetValue)
        {
            Name = SetText;
            Type = SetType;
            Value = SetValue;
        }
    }
}
