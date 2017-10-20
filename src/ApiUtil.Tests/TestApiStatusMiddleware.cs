using System.Net;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ApiUtil.Tests
{
    public class TestApiStatusMiddleware
    {
        private ITestOutputHelper _writer;

        public TestApiStatusMiddleware(ITestOutputHelper writer)
        {
            _writer = writer;
        }

        [Fact]
        public void Response_content_type_is_json()
        {
            Assert.Equal("application/json", ApiStatusMiddleware.DefaultResponseContentType);
        }

        [Fact]
        public void Has_correct_default_path()
        {
            Assert.Equal("/system/status", ApiStatusMiddlewareExtensions.DefaultPath);
        }

        [Fact]
        public async void Test1()
        {
            using (var server = TestServer.Create(app => { app.UseApiStatusMiddleware(); }))
            {
                var response = await server.HttpClient.GetAsync(ApiStatusMiddlewareExtensions.DefaultPath);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(ApiStatusMiddleware.DefaultResponseContentType, response.Content.Headers.ContentType.ToString());

                var value = await response.Content.ReadAsStringAsync();
                var apiStatus = JsonConvert.DeserializeObject<ApiVersion>(value);

                _writer.WriteLine(value);

                Assert.Equal(A.ApiStatus.Build(), apiStatus);
            }
        }

        [Fact]
        public async void Test2()
        {
            using (var server = TestServer.Create(app => { app.UseApiStatusMiddleware("/api/version"); }))
            {
                var response = await server.HttpClient.GetAsync("/api/version");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(ApiStatusMiddleware.DefaultResponseContentType, response.Content.Headers.ContentType.ToString());

                var apiStatus = JsonConvert.DeserializeObject<ApiVersion>(await response.Content.ReadAsStringAsync());

                Assert.Equal(A.ApiStatus.Build(), apiStatus);
            }
        }

        [Fact]
        public async void Test3()
        {
            using (var server = TestServer.Create(app => { app.UseApiStatusMiddleware("/api/version", new ApiStatus(new ApiVersion("v", "bn", "c"))); }))
            {
                var response = await server.HttpClient.GetAsync("/api/version");

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(ApiStatusMiddleware.DefaultResponseContentType, response.Content.Headers.ContentType.ToString());

                var s = await response.Content.ReadAsStringAsync();
                var apiStatus = JsonConvert.DeserializeObject<ApiVersion>(s);

                _writer.WriteLine(s);

                Assert.Equal(A.ApiStatus.WithVersion("v").WithBuildNumber("bn").WithCommit("c").Build(), apiStatus);
            }
        }
    }
}