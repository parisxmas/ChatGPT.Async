using net.core.business.DataAccessFolder;
using net.core.business.Helpers;
using net.core.business.Services.Base;
using net.core.business.StoredProcedures;
using net.core.data.StoredProcedureModels;

namespace net.core.business.Services.SendersServiceFolder;

public class SenderService : BaseService, ISenderService
{
    public SenderService(IConn conn) : base(conn)
    {
    }

    public List<SPGetSendersResponse> GetSenders()
    {
        var senders = SPHelper.ExecuteSP<SPGetSenders, List<SPGetSendersResponse>>(base.GetSPObject<SPGetSenders>());

        return senders;
    }
}