using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Events;
using Discord.WebSocket;
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

		internal static string[] VersionTag => new string[2] {ReleaseName, VersionName};

		internal static readonly DiscordShardedClient Bot = new DiscordShardedClient(ClientConfig.WebSocketConfig());

		internal static async Task Main() {
			Bot.ShardReady += ReadyEvent.Main;

			Bot.MessageReceived += (SocketMessage arg) => {
				throw new NotImplementedException();
			};

			Bot.UserJoined += (SocketGuildUser arg) => {
				throw new NotImplementedException();
			};

			Bot.UserLeft += (SocketGuildUser arg) => {
				throw new NotImplementedException();
			};

			Bot.Log += LogEvents.LogEvent;

			Bot.LoggedIn += LogEvents.LoggedInEvent;

			Bot.LoggedOut += LogEvents.LoggedOutEvent;

			Bot.ShardConnected += LogEvents.ConnectedEvent;

			Bot.ShardDisconnected += LogEvents.DisconnectedEvent;

			Bot.ShardLatencyUpdated += LogEvents.LatencyUpdated;

			await Bot.LoginAsync(Discord.TokenType.Bot, ClientConfig.Token).ConfigureAwait(false);

			await Bot.StartAsync().ConfigureAwait(false);

			AppDomain.CurrentDomain.ProcessExit += ConsoleExitEvent.Main;

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
