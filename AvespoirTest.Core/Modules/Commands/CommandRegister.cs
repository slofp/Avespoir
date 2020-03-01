using AvespoirTest.Core.Configs;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;

namespace AvespoirTest.Core.Modules.Commands {

	class CommandRegister {

		static DiscordClient Bot = Client.Bot;

		static CommandsNextModule CommandMainPrefix = Bot.UseCommandsNext(new CommandsNextConfiguration {
			//現時点で代替を模索中
			StringPrefix = CommandConfig.MainPrefix,
			EnableDms = false,
			EnableMentionPrefix = false
		});

		#region MessgaeEvent用
		internal static void PublicCommands(MessageCreateEventArgs Message_Objects) {
			if (Message_Objects.Author.IsBot) return;
			CommandMainPrefix.RegisterCommands<PublicCommands>();
		}

		internal static void ModeratorCommands(MessageCreateEventArgs Message_Objects) {
			if (Message_Objects.Author.IsBot) return;
			CommandMainPrefix.RegisterCommands<ModeratorCommands>();
		}

		internal static void BotownerCommands(MessageCreateEventArgs Message_Objects) {
			if (Message_Objects.Author.IsBot) return;
			CommandMainPrefix.RegisterCommands<BotownerCommands>();
		}
		#endregion

		#region NoMessage用
		internal static void PublicCommands() {
			CommandMainPrefix.RegisterCommands<PublicCommands>();
		}

		internal static void ModeratorCommands() {
			CommandMainPrefix.RegisterCommands<ModeratorCommands>();
		}

		internal static void BotownerCommands() {
			CommandMainPrefix.RegisterCommands<BotownerCommands>();
		}
		#endregion
	}
}
