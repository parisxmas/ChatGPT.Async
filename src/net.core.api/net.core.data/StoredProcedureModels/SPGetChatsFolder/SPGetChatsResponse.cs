namespace net.core.data.StoredProcedureModels;

public class SPGetChatsResponse
{
    public string Text { get; set; }
    public DateTime? SendDate { get; set; }
    public Guid? SenderId { get; set; }
}