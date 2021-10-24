using DSharpPlus;
using Microsoft.Extensions.Logging;

namespace Avespoir.Core.Configs {

	class ClientConfig {

		internal static string Token { get; set; } = "";

		internal static ulong BotownerId { get; set; } = 0;

		internal static DiscordConfiguration WebSocketConfig() => new DiscordConfiguration() {
			Intents = DiscordIntents.All,
			TokenType = TokenType.Bot,
			Token = Token,
			#if DEBUG
			MinimumLogLevel = LogLevel.Trace
			#endif
		};
	}
}
