using AvespoirTest.Core.Configs;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace AvespoirTest.Core.Modules.Commands {

	class CommandRegister {

		static DiscordClient Bot = Client.Bot;

		static CommandsNextModule CommandMainPrefix = Bot.UseCommandsNext(new CommandsNextConfiguration {
			//現時点で代替を模索中
			StringPrefix = CommandConfig.PublicPrefix,
			EnableDms = false,
			EnableMentionPrefix = false
		});

		internal static void PublicCommands(MessageCreateEventArgs Message_Objects) {
			CommandMainPrefix.RegisterCommands<PublicCommands>();
		}

		internal static void ModeratorCommands(MessageCreateEventArgs Message_Objects) {
			CommandMainPrefix.RegisterCommands<ModeratorCommands>();
		}

		internal static void BotownerCommands(MessageCreateEventArgs Message_Objects) {
			CommandMainPrefix.RegisterCommands<BotownerCommands>();
		}
	}
}
