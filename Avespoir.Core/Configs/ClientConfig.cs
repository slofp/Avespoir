using Discord;
using Discord.WebSocket;

namespace Avespoir.Core.Configs {

	class ClientConfig {

		internal static string Token { get; set; } = "";

		internal static ulong BotownerId { get; set; } = 0;

		internal static DiscordSocketConfig WebSocketConfig() => new DiscordSocketConfig() {
			#if DEBUG
			DefaultRetryMode = RetryMode.AlwaysFail,
			LogLevel = LogSeverity.Debug
			#else
			DefaultRetryMode = RetryMode.AlwaysRetry,
			LogLevel = LogSeverity.Info
			#endif
		};
	}
}
