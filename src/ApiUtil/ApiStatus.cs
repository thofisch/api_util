using System;
using System.Diagnostics.CodeAnalysis;

namespace ApiUtil
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ApiStatus
    {
        public ApiStatus()
        {
            UpSince = DateTimeOffset.Now.ToString("O");
        }

        internal ApiStatus(ApiVersion version) : this()
        {
            Version = version.Version;
            BuildNumber = version.BuildNumber;
            Commit = version.Commit;
        }

        public string UpSince { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
        public string Commit { get; set; }
    }
}