using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace ApiUtil
{
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal sealed class ApiVersion
    {
        public const string NotAvailableToken = "n/a";

        public static readonly ApiVersion Empty = new ApiVersion(NotAvailableToken, NotAvailableToken, NotAvailableToken);

        public static ApiVersion For(Assembly assembly)
        {
            var informationalVersion = GetInformationalVersion(assembly);

            return ParseInformationalVersion(informationalVersion);
        }

        private static string GetInformationalVersion(ICustomAttributeProvider assembly)
        {
            var attrs = (AssemblyInformationalVersionAttribute[]) assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), true);
            return attrs.Select(x => x.InformationalVersion).SingleOrDefault() ?? string.Empty;
        }

        internal static ApiVersion ParseInformationalVersion(string informationalVersion)
        {
            var tokens = informationalVersion?.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens?.Length != 3)
            {
                return Empty;
            }

            var version = tokens[0];
            var buildNumber = tokens[1];
            var commit = tokens[2];

            return new ApiVersion(version, buildNumber, commit);
        }

        public ApiVersion(string version, string buildNumber, string commit)
        {
            Version = version ?? throw new ArgumentNullException(nameof(version));
            BuildNumber = buildNumber ?? throw new ArgumentNullException(nameof(buildNumber));
            Commit = commit ?? throw new ArgumentNullException(nameof(commit));
        }

        public string Version { get; }
        public string BuildNumber { get; }
        public string Commit { get; }

        private bool Equals(ApiVersion other)
        {
            return string.Equals(Version, other.Version) && string.Equals(BuildNumber, other.BuildNumber) && string.Equals(Commit, other.Commit);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            var apiVersion = obj as ApiVersion;
            return apiVersion != null && Equals(apiVersion);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Version.GetHashCode();
                hashCode = (hashCode * 397) ^ BuildNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ Commit.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{Version}-{BuildNumber}-{Commit}";
        }
    }
}