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

    public Task<List<SPGetChatsResponse>> GetChats(GetChatsRequest request)
    {
        return Task.Run(() =>
        {
            var chats = SPHelper.ExecuteSP<SPGetChats, List<SPGetChatsResponse>>(base.GetSPObject<SPGetChats>(),
                new SPGetChatsRequest()
                {
                    Search = request.Search,
                    Page = request.Page,
                    PageSize = request.PageSize
                });

            return chats;
        });
    }

    public Task InsertMessage(InsertMessageRequest request)
    {
        return Task.Run(() =>
        {
            SPHelper.ExecuteSP<SPInsertMessage, object>(base.GetSPObject<SPInsertMessage>(),
                new SPInsertMessageRequest()
                {
                    SenderId = request.SenderId,
                    Text = request.Text
                });
        });
    }
}