namespace Contracts.Common.BaseResponse
{
    public class BaseResponse<T> where T : class
    {
        public T? Result { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
