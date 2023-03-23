namespace net.core.data.StoredProcedureModels;

public class SPInsertMessageRequest
{
    public string Text { get; set; }
    public Guid SenderId { get; set; }
}