using Avespoir.Core.Configs;
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
					Log.Critical($"Normal Log: {BotLogMessage.Message}", BotLogMessage.Exception);
					break;
				case LogSeverity.Debug:
					Log.Debug($"Normal Log: {BotLogMessage.Message}", BotLogMessage.Exception);
					break;
				case LogSeverity.Verbose:
					Log.Verbose($"Normal Log: {BotLogMessage.Message}", BotLogMessage.Exception);
					break;
				case LogSeverity.Error:
					Log.Error($"Normal Log: {BotLogMessage.Message}", BotLogMessage.Exception);
					break;
				case LogSeverity.Warning:
					Log.Warning($"Normal Log: {BotLogMessage.Message}", BotLogMessage.Exception);
					break;
				case LogSeverity.Info:
					Log.Info($"Normal Log: {BotLogMessage.Message}", BotLogMessage.Exception);
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

		internal static async Task DisconnectedEvent(Exception Error, DiscordSocketClient Bot) {
			Log.Info($"ShardID: {Bot.ShardId} Disconnected!");
			if (!(Error is null)) {
				Log.Error("Disconnection due to error", Error);
				if (!ReadyEvent.ExitCheck) {
					Log.Info("Reconnecting...");
					ReadyEvent.ExitCheck = true;
					await Task.Delay(1000).ConfigureAwait(false);

					await Client.Bot.StopAsync().ConfigureAwait(false);
					await Client.Bot.LogoutAsync().ConfigureAwait(false);

					ReadyEvent.ExitCheck = false;

					await Client.Bot.LoginAsync(TokenType.Bot, ClientConfig.Token).ConfigureAwait(false);
					await Client.Bot.StartAsync().ConfigureAwait(false);
				}
			}
		}

		internal static Task LatencyUpdated(int Ping1, int Ping2, DiscordSocketClient Bot) {
			Log.Info($"ShardID: {Bot.ShardId}, Ping: {Ping1}ms {Ping2}ms");
			return Task.CompletedTask;
		}
	}
}
