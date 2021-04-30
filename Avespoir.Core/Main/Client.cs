using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class Client {

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

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		internal static async Task Main() {
			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			Bot.ClientErrored += ClientErroredEvent.Main;

			//Bot.Logger.
			//Bot.DebugLogger.LogMessageReceived += (Sender, LogMessage) => Log.Info(LogMessage.Message);
			//#if !DEBUG
			Bot.Heartbeated += Avespoir.Core.Modules.Logger.HeartbeatLog.ExportHeartbeatLog;
			//#endif

			await Bot.ConnectAsync();

			AppDomain.CurrentDomain.ProcessExit += ConsoleExitEvent.Main;

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
