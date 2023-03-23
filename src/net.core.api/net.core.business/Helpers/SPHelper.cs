using System.Reflection;
using net.core.data.Base;
using net.core.data.Exceptions;
using net.core.data.StoredProcedureModels.Base;

namespace net.core.business.Helpers;

public static class SPHelper
{

    public static TOut ExecuteSP<TObj, TOut>(TObj classInstance, object prms= null)
    {
        Type classType = classInstance.GetType();
        string className = classType.Name;
        string responseClassName = className + "Response";

        Assembly assembly = Assembly.Load(GLOBALS.DATA_PROJECT_ASSEMBLY_NAME);
        Type responseType = assembly.GetType(GLOBALS.StoredProcedureModelsNamespacePath + $".{responseClassName}");
       
        var rslt = Activator.CreateInstance(responseType);


        if (classType != null)
        {
            MethodInfo methodInfo = classType.GetMethod("Process");
            if (methodInfo != null)
            {
                ParameterInfo[] parameters = methodInfo.GetParameters();
                object[] parametersArray = new object[] { prms };

                rslt = methodInfo.Invoke(classInstance, parametersArray);

                var innerResultObj = (List<Result>)rslt.GetType().GetProperty("Result").GetValue(rslt, null);

                if (ResultControl.Error(innerResultObj))
                {
                    string errorMessage = $"Error executing {classType.Name}!";
                    innerResultObj.ForEach(each => errorMessage += ($" SP Error Detail: {each.Detail}\n"));

                    throw new AppException(errorMessage);
                }
                else
                {
                    return (TOut)rslt.GetType().GetProperty("List").GetValue(rslt, null);
                }
            }
        }

        return default;
    }
}