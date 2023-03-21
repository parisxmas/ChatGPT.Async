using System.Data;
using net.core.business.DataAccessFolder;
using net.core.business.StoredProcedures.Base;
using net.core.data.DataAccessFolder;
using net.core.data.StoredProcedureModels.Base;
using net.core.data.StoredProcedureModels;

namespace net.core.business.StoredProcedures;

public class SPGetChats : SPBase
{
    public SPGetChats(IConn conn) : base(conn)
    {
    }

    public BaseSPResponse<SPGetChatsResponse> Process(SPGetChatsRequest requestItem)
    {
        BaseSPResponse<SPGetChatsResponse> result = new BaseSPResponse<SPGetChatsResponse>();

        List<SQLParameterItem> Params = new List<SQLParameterItem>();
        if (requestItem != null)
        {
            Params.Add(new SQLParameterItem("Search", SqlColumnType.NVarChar, requestItem.Search));
            Params.Add(new SQLParameterItem("Page", SqlColumnType.Int, requestItem.Page));
            Params.Add(new SQLParameterItem("PageSize", SqlColumnType.Int, requestItem.PageSize));
        }

        DataTable dt = base._sqlEngine.Run("SP_GetChats", Params).Table;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Columns.Contains("ErrorId") == false)
            {
                result.List = (List<SPGetChatsResponse>)dt.AsEnumerable().Select(row =>
                    new SPGetChatsResponse
                    {
                        SendDate = (row["SendDate"] != DBNull.Value ? row.Field<DateTime?>("SendDate") : null),
                        SenderId = (row["SenderId"] != DBNull.Value ? row.Field<Guid?>("SenderId") : null),
                        Text = (row["Text"] != DBNull.Value ? row.Field<String?>("Text") : null)
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