using DSharpPlus;
using Microsoft.Extensions.Logging;

namespace Avespoir.Core.Configs {

	class ClientConfig {

		internal static string Token { get; set; } = "";

		internal static ulong BotownerId { get; set; } = 0;

		internal static DiscordConfiguration DiscordConfig() => new DiscordConfiguration {
			Token = Token,
			Intents = DiscordIntents.All,
			LoggerFactory = new LoggerFactory(), // No Display Log // I hate this.
			#if DEBUG
			MinimumLogLevel = LogLevel.Trace
			#endif
		};

	}
}
