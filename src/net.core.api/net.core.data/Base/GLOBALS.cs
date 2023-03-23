namespace net.core.data.Base
{
    public static class GLOBALS
    {
        public static readonly string SENDERS_CACHE_KEY = "SENDERS";
        public static readonly string SENDERS_USER = "User";
        public static readonly string SENDERS_SYSTEM = "System";

        public static readonly string BASE_NAMESPACE_PATH = "net.core";
        public static string DATA_PROJECT_ASSEMBLY_NAME = $"{BASE_NAMESPACE_PATH}.data";

        public static string DatabaseConnectorNamespacePath = $"{BASE_NAMESPACE_PATH}.business";
        public static string StoredProcedureModelsNamespacePath = $"{DATA_PROJECT_ASSEMBLY_NAME}.StoredProcedureModels";
    }
}