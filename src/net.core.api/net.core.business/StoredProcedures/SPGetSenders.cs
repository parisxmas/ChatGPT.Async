using System.Data;
using net.core.business.DataAccessFolder;
using net.core.business.StoredProcedures.Base;
using net.core.data.DataAccessFolder;
using net.core.data.StoredProcedureModels.Base;
using net.core.data.StoredProcedureModels;

namespace net.core.business.StoredProcedures;

public class SPGetSenders:SPBase
{
    public SPGetSenders(IConn conn) : base(conn)
    {

    }
    public BaseSPResponse<SPGetSendersResponse> Process(object parameter)
    {
        BaseSPResponse<SPGetSendersResponse> result = new  BaseSPResponse<SPGetSendersResponse>();

        List<SQLParameterItem> Params = new List<SQLParameterItem>();

        DataTable dt = base._sqlEngine.Run("SP_GetSenders", Params).Table;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("ErrorId") == false)
            {
                result.List = (List<SPGetSendersResponse>)dt.AsEnumerable().Select(row =>
                    new SPGetSendersResponse
                    {
                        Id = (row["Id"] != DBNull.Value ? row.Field<Guid?>("Id") : null),
                        Name = (row["Name"] != DBNull.Value ? row.Field<String?>("Name") : null)
                    }).ToList();
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Result item = new Result();
                    if (dr["Detail"] != DBNull.Value) item.Detail = (String)dr["Detail"];
                    if (dr["RecordId"] != DBNull.Value) item.RecordId = (Int32)dr["RecordId"];
                    if (dr["ErrorId"] != DBNull.Value) item.ErrorId = (Int32)dr["ErrorId"];
                    if (dr["Error"] != DBNull.Value) item.Error = (Boolean)dr["Error"];
                    result.Result.Add(item);
                }
            }
        }

        return result;
    }

}