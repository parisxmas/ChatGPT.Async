using net.core.business.DataAccessFolder;
using net.core.business.Helpers;
using net.core.business.Services.Base;
using net.core.business.StoredProcedures;
using net.core.data.Services.ChatService;
using net.core.data.StoredProcedureModels;

namespace net.core.business.Services.ChatServiceFolder;

public class ChatService : BaseService, IChatService
{
    public ChatService(IConn conn) : base(conn)
    {
    }

    public List<SPGetChatsResponse> GetChats(GetChatsRequest request)
    {
        var chats = SPHelper.ExecuteSP<SPGetChats, List<SPGetChatsResponse>>(base.GetSPObject<SPGetChats>(),
            new SPGetChatsRequest()
            {
                Search = request.Search,
                Page = request.Page,
                PageSize = request.PageSize
            });

        return chats;
    }

    public void InsertMessage(InsertMessageRequest request)
    {
        SPHelper.ExecuteSP<SPInsertMessage, object>(base.GetSPObject<SPInsertMessage>(),
            new SPInsertMessageRequest()
            {
                SenderId = request.SenderId,
                Text = request.Text
            });
    }
}