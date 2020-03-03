using DSharpPlus;
using System;

namespace AvespoirTest.Core.Modules.Logger {

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
					if (Bot.CurrentUser.Username == "" || Bot.CurrentUser.Username == null) return "Bot";
					return Bot.CurrentUser.Username;
				}
				catch (NullReferenceException) {
					return "Bot";
				}
			}
		}
	}
}
