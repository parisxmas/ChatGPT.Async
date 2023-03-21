using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.core.data.StoredProcedureModels.Base
{
    public class Result
    {
        public String Detail { get; set; }
        public Int32 RecordId { get; set; }
        public Int32 ErrorId { get; set; }
        public Boolean Error { get; set; }
    }
}
