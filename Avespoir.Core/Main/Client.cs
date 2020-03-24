using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class Client {
		internal static readonly string Version = "Beta 1.1";

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		internal static async Task Main() {

			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			Bot.ClientErrored += ClientErroredEvent.Main;

			#if !DEBUG
			Bot.DebugLogger.LogMessageReceived += (Sender, Log) => Console.WriteLine(Log);
			Bot.Heartbeated += Avespoir.Core.Modules.Logger.HeartbeatLog.ExportHeartbeatLog;
			#endif

			Bot.DebugLogger.LogMessageReceived += ClientLog.ExportLog;

			await Bot.ConnectAsync();

			Console.CancelKeyPress += ConsoleExitEvent;

			await Task.Delay(-1);
		}

		static void ConsoleExitEvent(object Sender, ConsoleCancelEventArgs Args) {
			new InfoLog("Exit...");

			Bot.DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
			Bot.Dispose();

			Environment.Exit(Environment.ExitCode);
		}
	}
}
