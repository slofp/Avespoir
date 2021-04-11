using DSharpPlus;

namespace Avespoir.Core.Configs {

	class ClientConfig {

		internal static string Token { get; set; } = "";

		internal static ulong BotownerId { get; set; } = 0;
		
		internal static DiscordConfiguration DiscordConfig() => new DiscordConfiguration {
			Token = Token,
			//LoggerFactory = Modules.Logger.LoggerProperties.Log.Logger.
			//#if DEBUG
			//MinimumLogLevel = LogLevel.Debug
			//#endif
		};

	}
}
