namespace net.core.data.Base
{
    public class ResponseObject<T>
    {
        public bool IsSuccess { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
