using net.core.data.StoredProcedureModels.Base;

namespace net.core.business.Helpers;

public static class ResultControl
{
    public static bool Error(List<Result> resultList)
    {
        bool result = false;
        if (resultList.Where(x => x.Error == true).Count() > 0) result = true;
        return result;
    }

    public static bool Error(Result resultItem)
    {
        return resultItem.Error;
    }

    public static bool Exist(dynamic models)
    {
        bool result = false;
        try
        {
            if (((List<Result>)models.Result).Where(x => x.Error == true).Count() == 0)
            {
                if (models.List != null && models.List.Count > 0) result = true;
            }
        }
        catch { }

        return result;
    }
}