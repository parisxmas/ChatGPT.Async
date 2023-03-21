using System.Data;
using net.core.business.DataAccessFolder;
using net.core.business.StoredProcedures.Base;
using net.core.data.DataAccessFolder;
using net.core.data.StoredProcedureModels.Base;
using net.core.data.StoredProcedureModels;

namespace net.core.business.StoredProcedures;

public class SPInsertMessage:SPBase
{
    public SPInsertMessage(IConn conn) : base(conn)
    {
    }

    public BaseSPResponse<SPInsertMessageResponse> Process(SPInsertMessageRequest requestItem)
    {
        BaseSPResponse<SPInsertMessageResponse> result = new BaseSPResponse<SPInsertMessageResponse>();

        List<SQLParameterItem> Params = new List<SQLParameterItem>();
        if (requestItem != null)
        {
            Params.Add(new SQLParameterItem("Text", SqlColumnType.NVarChar, requestItem.Text));
            Params.Add(new SQLParameterItem("SenderId", SqlColumnType.UniqueIdentifier, requestItem.SenderId));
        }

        DataTable dt = base._sqlEngine.Run("SP_InsertMessage", Params).Table;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("ErrorId") == false)
            {
                result.List = (List<SPInsertMessageResponse>)dt.AsEnumerable().Select(row =>
                    new SPInsertMessageResponse
                    {
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