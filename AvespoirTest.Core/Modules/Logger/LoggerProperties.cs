using DSharpPlus;

namespace AvespoirTest.Core.Modules.Logger {

	class LoggerProperties {
		
		static DiscordClient Bot {
			get {
				return Client.Bot;
			}
		}

		internal static DebugLogger Debug_Logger {
			get {
				return Bot.DebugLogger;
			}
		}

		internal static string Username {
			get {
				return Bot.CurrentUser.Username ?? "Bot";
			}
		}
	}
}
