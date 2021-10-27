using Avespoir.Core.Modules.Logger;
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
			LoggerFactory = LoggerFactory.Create(conf => {
				conf.ClearProviders();
				conf.AddProvider(new Log4NetProvider());
				conf.SetMinimumLevel(LogLevel.Critical);
			}),
			#if DEBUG
			MinimumLogLevel = LogLevel.Trace
			#endif
		};
	}
}
