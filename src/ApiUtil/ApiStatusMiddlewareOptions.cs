namespace ApiUtil
{
    internal class ApiStatusMiddlewareOptions
    {
        public ApiStatusMiddlewareOptions(ApiStatus status)
        {
            Status = status;
        }

        public ApiStatus Status { get; }
    }
}