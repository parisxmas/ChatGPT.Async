using net.core.data.Services.ChatService;
using net.core.data.StoredProcedureModels;

namespace net.core.business.Services.ChatServiceFolder;

public interface IChatService
{
    List<SPGetChatsResponse> GetChats(GetChatsRequest request);

    void InsertMessage(InsertMessageRequest request);
}