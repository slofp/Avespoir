using DSharpPlus;
using System;

namespace Avespoir.Core.Modules.Logger {

	class LoggerProperties {

		static DiscordClient Bot = Client.Bot;

		internal static DebugLogger Debug_Logger {
			get {
				return Bot.DebugLogger;
			}
		}
		
		internal static string Username {
			get {
				try {
					if (string.IsNullOrWhiteSpace(Bot.CurrentUser.Username)) return "Bot";
					else return Bot.CurrentUser.Username;
				}
				catch (NullReferenceException) {
					return "Bot";
				}
			}
		}
	}
}
