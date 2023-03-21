namespace net.core.data.Services.ChatService;

public class GetChatsRequest
{
    public string Search { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}