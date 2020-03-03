using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Commands;
using AvespoirTest.Core.Modules.Events;
using DSharpPlus;
using System.Threading;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	class Client {

		internal Client(string[] args) => Main(args).ConfigureAwait(false).GetAwaiter().GetResult();

		internal static DiscordClient Bot = new DiscordClient(ClientConfig.DiscordConfig());

		async Task Main(string[] args) {
			new ClientLog().StartClientLogEvents();

			Bot.Ready += ReadyEvent.Main;

			Bot.MessageCreated += MessageEvent.Main;

			Bot.GuildMemberAdded += GuildMemberAddEvent.Main;

			Bot.GuildMemberRemoved += GuildMemberRemoveEvent.Main;

			CommandRegister.PublicCommands();

			CommandRegister.ModeratorCommands();

			CommandRegister.BotownerCommands();

			await Bot.ConnectAsync();

			await Task.Delay(-1, CancellationToken.None);
		}
	}
}
