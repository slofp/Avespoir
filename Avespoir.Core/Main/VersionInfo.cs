using System;
using System.Reflection;

namespace Avespoir.Core.Main {

	class VersionInfo {

		private static readonly AssemblyName AssemblyInfo = Assembly.GetExecutingAssembly().GetName();

		private static readonly Version AssemblyVersion = AssemblyInfo.Version;

		private static string ProjectName => AssemblyVersion.Major switch {
			1 => "Silence",
			2 => "Avespoir",
			_ => "Unknown",
		};

		private static string ReleaseName => AssemblyVersion.Minor switch {
			0 => "Alpha",
			1 => "Beta",
			2 => "Stable",
			_ => "Unknown",
		};

		private static string VersionName => string.Format("{0}.{1}", AssemblyVersion.Build, AssemblyVersion.Revision);

		#if DEBUG
		private const string BuildTypeName = " Dev";
		#else
		private const string BuildTypeName = "";
		#endif

		internal static string Version => string.Format("{0} {1} {2}{3}", ProjectName, ReleaseName, VersionName, BuildTypeName);

		internal static string[] VersionTag => new string[2] { ReleaseName, VersionName };
	}
}
