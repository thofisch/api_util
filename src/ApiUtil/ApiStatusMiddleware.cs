using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiUtil
{
    internal class ApiStatusMiddleware : OwinMiddleware
    {
        public const string DefaultResponseContentType = "application/json";

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly ApiStatusMiddlewareOptions _options;

        public ApiStatusMiddleware(OwinMiddleware next, ApiStatusMiddlewareOptions options) : base(next)
        {
            _options = options;
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Response.ContentType = DefaultResponseContentType;

            var apiStatus = _options.Status;

            var json = JsonConvert.SerializeObject(apiStatus, Formatting.Indented, JsonSerializerSettings);
            await context.Response.WriteAsync(json);
        }
    }
}