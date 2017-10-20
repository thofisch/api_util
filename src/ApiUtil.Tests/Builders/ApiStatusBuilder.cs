namespace ApiUtil.Tests.Builders
{
    internal class ApiStatusBuilder
    {
        private string _version = "version";
        private string _buildNumber = "build_number";
        private string _commit = "commit";

        public ApiStatusBuilder WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public ApiStatusBuilder WithBuildNumber(string buildNumber)
        {
            _buildNumber = buildNumber;
            return this;
        }

        public ApiStatusBuilder WithCommit(string commit)
        {
            _commit = commit;
            return this;
        }

        public ApiVersion Build()
        {
            return new ApiVersion(_version, _buildNumber, _commit);
        }

        public static implicit operator ApiVersion(ApiStatusBuilder builder)
        {
            return builder.Build();
        }
    }
}