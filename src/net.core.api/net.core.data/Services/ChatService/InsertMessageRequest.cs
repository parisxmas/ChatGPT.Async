namespace net.core.data.Services.ChatService;

public class InsertMessageRequest
{
    public string Text { get; set; }
    public Guid SenderId { get; set; }
}