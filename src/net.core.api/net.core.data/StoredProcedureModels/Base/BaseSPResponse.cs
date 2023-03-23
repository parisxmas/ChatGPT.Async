using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.core.data.StoredProcedureModels.Base
{
    public class BaseSPResponse<T> where T : class
    {
        public List<Result> Result { get; set; } = new List<Result>();
        public List<T> List { get; set; } = new List<T>();
    }
}
