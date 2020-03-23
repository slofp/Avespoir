using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Events;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	class Client {
		internal static readonly string Version = "Alpha 4.0 (Final Update)";

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		internal static async Task Main() {

			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			Bot.ClientErrored += ClientErroredEvent.Main;

			#if !DEBUG
			Bot.DebugLogger.LogMessageReceived += (Sender, Log) => Console.WriteLine(Log);
			Bot.Heartbeated += HeartbeatLog.ExportHeartbeatLog;
			#endif

			Bot.DebugLogger.LogMessageReceived += ClientLog.ExportLog;

			await Bot.ConnectAsync();

			await Task.Delay(-1);
		}
	}
}
