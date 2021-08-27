using Avespoir.Core.Modules.Logger;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class LogEvents {

		internal static Task LogEvent(LogMessage BotLogMessage) {
			switch (BotLogMessage.Severity) {
				case LogSeverity.Critical:
					Log.Critical(BotLogMessage.Message, BotLogMessage.Exception);
					break;
				case LogSeverity.Debug:
					Log.Debug(BotLogMessage.Message, BotLogMessage.Exception);
					break;
				case LogSeverity.Verbose:
					Log.Verbose(BotLogMessage.Message, BotLogMessage.Exception);
					break;
				case LogSeverity.Error:
					Log.Error(BotLogMessage.Message, BotLogMessage.Exception);
					break;
				case LogSeverity.Warning:
					Log.Warning(BotLogMessage.Message, BotLogMessage.Exception);
					break;
				case LogSeverity.Info:
					Log.Info(BotLogMessage.Message, BotLogMessage.Exception);
					break;
			}

			return Task.CompletedTask;
		}

		internal static Task LoggedInEvent() {
			Log.Info("Bot Logged in!");
			return Task.CompletedTask;
		}

		internal static Task LoggedOutEvent() {
			Log.Info("Bot Logged out!");
			return Task.CompletedTask;
		}

		internal static Task ConnectedEvent(DiscordSocketClient Bot) {
			Log.Info($"ShardID: {Bot.ShardId} Connected!");
			return Task.CompletedTask;
		}

		internal static Task DisconnectedEvent(Exception Error, DiscordSocketClient Bot) {
			Log.Info($"ShardID: {Bot.ShardId} Disconnected!");
			if (!(Error is null)) {
				Log.Error("Disconnection due to error", Error);
			}
			return Task.CompletedTask;
		}

		internal static Task LatencyUpdated(int Ping1, int Ping2, DiscordSocketClient Bot) {
			Log.Info($"ShardID: {Bot.ShardId}, Ping: {Ping1}ms {Ping2}ms");
			return Task.CompletedTask;
		}
	}
}
