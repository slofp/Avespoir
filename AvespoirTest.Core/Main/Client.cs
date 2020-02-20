using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Commands;
using AvespoirTest.Core.Modules.Message;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Threading;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	class Client {

		internal Client(string[] args) => MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		internal static CommandsNextModule Public_Commands = Bot.UseCommandsNext(new CommandsNextConfiguration {
			StringPrefix = CommandConfig.PublicPrefix
		});

		//internal static CommandsNextModule Moderator_Commands = Bot.UseCommandsNext(new CommandsNextConfiguration {
		//	StringPrefix = CommandConfig.ModeratorPrefix
		//});

		//internal static CommandsNextModule Botowner_Commands = Bot.UseCommandsNext(new CommandsNextConfiguration {
		//	StringPrefix = CommandConfig.BotownerPrefix
		//});

		async Task MainAsync(string[] args) {
			new ClientLog();

			new MessageCreated();

			await Bot.ConnectAsync();

			CommandRegister.PublicCommands();

			//CommandRegister.ModeratorCommands();

			//CommandRegister.BotownerCommands();

			await Task.Delay(-1, CancellationToken.None);
		}
	}
}
