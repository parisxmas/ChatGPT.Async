namespace net.core.data.StoredProcedureModels;

public class SPGetChatsRequest
{
    public string Search { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}