using Avespoir.Core.Modules.Logger;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class LogEvents {

		internal static Task LogEvent(LogMessage BotLogMessage) {
			if (BotLogMessage.Exception is null) {
				Log.Error(BotLogMessage.Message, BotLogMessage.Exception);
			}
			else {
				switch (BotLogMessage.Severity) {
					case LogSeverity.Critical:
						Log.Critical(BotLogMessage.Message);
						break;
					case LogSeverity.Debug:
						Log.Debug(BotLogMessage.Message);
						break;
					case LogSeverity.Verbose:
						Log.Error(BotLogMessage.Message);
						break;
					case LogSeverity.Error:
						Log.Error(BotLogMessage.Message);
						break;
					case LogSeverity.Warning:
						Log.Warning(BotLogMessage.Message);
						break;
					case LogSeverity.Info:
						Log.Info(BotLogMessage.Message);
						break;
				}
			}

			return Task.CompletedTask;
		}

		internal static Task LoggedInEvent() {
			return Task.CompletedTask;
		}

		internal static Task LoggedOutEvent() {
			return Task.CompletedTask;
		}

		internal static Task ConnectedEvent(DiscordSocketClient Bot) {
			return Task.CompletedTask;
		}

		internal static Task DisconnectedEvent(Exception Error, DiscordSocketClient Bot) {
			return Task.CompletedTask;
		}

		internal static Task LatencyUpdated(int Ping1, int Ping2, DiscordSocketClient Bot) {
			return Task.CompletedTask;
		}
	}
}
