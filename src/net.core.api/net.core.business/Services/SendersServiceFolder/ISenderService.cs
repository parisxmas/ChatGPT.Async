using net.core.data.StoredProcedureModels;

namespace net.core.business.Services.SendersServiceFolder;

public interface ISenderService
{
    Task<List<SPGetSendersResponse>> GetSenders();
}