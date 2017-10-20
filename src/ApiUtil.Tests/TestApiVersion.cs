using System.Reflection;
using Xunit;

[assembly: AssemblyInformationalVersion("version-build_number-commit")]

namespace ApiUtil.Tests
{
    public class TestApiVersion
    {
        [Fact]
        public void Has_correct_not_available_token()
        {
            Assert.Equal("n/a", ApiVersion.NotAvailableToken);
        }

        [Fact]
        public void Can_get_informational_version()
        {
            var version = ApiVersion.For(GetType().Assembly);

            Assert.Equal(A.ApiStatus.Build(), version);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("-")]
        [InlineData("--")]
        [InlineData("---")]
        [InlineData("version-build_number-")]
        [InlineData("version--commit")]
        [InlineData("version--")]
        [InlineData("-build_number-")]
        [InlineData("-build_number-commit")]
        [InlineData("--commit")]
        public void Can_handle_malformed_informational_version(string informationalVersion)
        {
            var apiVersion = ApiVersion.ParseInformationalVersion(informationalVersion);

            Assert.Equal(ApiVersion.Empty, apiVersion);
        }

        [Fact]
        public void Can_get_descriptive_version_string()
        {
            var apiVersion = ApiVersion.ParseInformationalVersion("version-build_number-commit");

            Assert.Equal(A.ApiStatus.Build(), apiVersion);
        }
    }
}