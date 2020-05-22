using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class Client {
		internal static readonly string Version = "Beta 1.3";

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		internal static async Task Main() {

			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			Bot.ClientErrored += ClientErroredEvent.Main;

			Bot.DebugLogger.LogMessageReceived += (Sender, LogMessage) => Log.Info(LogMessage.Message);
			#if !DEBUG
			Bot.Heartbeated += Avespoir.Core.Modules.Logger.HeartbeatLog.ExportHeartbeatLog;
			#endif

			await Bot.ConnectAsync();

			Console.CancelKeyPress += ConsoleExitEvent.Main;

			await Task.Delay(-1);
		}
	}
}
