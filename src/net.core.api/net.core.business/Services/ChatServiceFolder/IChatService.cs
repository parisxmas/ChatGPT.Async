using net.core.data.Services.ChatService;
using net.core.data.StoredProcedureModels;

namespace net.core.business.Services.ChatServiceFolder;

public interface IChatService
{
    Task<List<SPGetChatsResponse>> GetChats(GetChatsRequest request);

    Task InsertMessage(InsertMessageRequest request);
}