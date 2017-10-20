using System.Reflection;
using Owin;

namespace ApiUtil
{
    public static class ApiStatusMiddlewareExtensions
    {
        internal const string DefaultPath = "/system/status";

        public static void UseApiStatusMiddleware(this IAppBuilder app)
        {
            var version = ApiVersion.For(Assembly.GetCallingAssembly());
            var status = new ApiStatus(version);

            app.UseApiStatusMiddleware(DefaultPath, status);
        }

        public static void UseApiStatusMiddleware(this IAppBuilder app, string pathMatch)
        {
            var version = ApiVersion.For(Assembly.GetCallingAssembly());
            var status = new ApiStatus(version);

            app.UseApiStatusMiddleware(pathMatch, status);
        }

        public static void UseApiStatusMiddleware(this IAppBuilder app, string pathMatch, ApiStatus apiStatus)
        {
            app.Map(pathMatch, builder => builder.Use<ApiStatusMiddleware>(new ApiStatusMiddlewareOptions(apiStatus)));
        }
    }
}