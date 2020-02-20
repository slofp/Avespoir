using DSharpPlus;
using DSharpPlus.CommandsNext;

namespace AvespoirTest.Core.Modules.Commands {

	class CommandRegister {

		internal static void PublicCommands() => Client.Public_Commands.RegisterCommands<PublicCommands>();

		// 何故か複数のPrefixが反応しない

		//internal static void ModeratorCommands() => Client.Moderator_Commands.RegisterCommands<ModeratorCommands>();

		//internal static void BotownerCommands() => Client.Botowner_Commands.RegisterCommands<BotownerCommands>();
	}
}
