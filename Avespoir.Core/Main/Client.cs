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

		internal static string[] VersionTag => new string[] {ReleaseName, VersionName};

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		internal static async Task Main() {
			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			Bot.ClientErrored += ClientErroredEvent.Main;

			Bot.UnknownEvent += (DiscordClient Client, DSharpPlus.EventArgs.UnknownEventArgs UnknownEvent) => {
				Log.Warning($"Unknown Event: {UnknownEvent.EventName}\nHandled: {UnknownEvent.Handled}\nJson: {UnknownEvent.Json}");

				return Task.CompletedTask;
			};

			Bot.SocketClosed += (DiscordClient Client, DSharpPlus.EventArgs.SocketCloseEventArgs SocketCloseEvent) => {
				Log.Info($"Socket Closed: {SocketCloseEvent.CloseCode}\nCloseMessage: {SocketCloseEvent.CloseMessage}\nHandled: {SocketCloseEvent.Handled}");

				return Task.CompletedTask;
			};

			Bot.SocketErrored += (DiscordClient Client, DSharpPlus.EventArgs.SocketErrorEventArgs SocketErrorEvent) => {
				Log.Error($"Socket Error\nHandled: {SocketErrorEvent.Handled}", SocketErrorEvent.Exception);

				return Task.CompletedTask;
			};

			Bot.SocketOpened += (DiscordClient Client, DSharpPlus.EventArgs.SocketEventArgs SocketEvent) => {
				Log.Info($"Socket Opened\nHandled: {SocketEvent.Handled}");

				return Task.CompletedTask;
			};

			//Bot.Logger.
			//Bot.DebugLogger.LogMessageReceived += (Sender, LogMessage) => Log.Info(LogMessage.Message);
			//#if !DEBUG
			Bot.Heartbeated += HeartbeatLog.ExportHeartbeatLog;
			//#endif

			await Bot.ConnectAsync();

			AppDomain.CurrentDomain.ProcessExit += ConsoleExitEvent.Main;

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
