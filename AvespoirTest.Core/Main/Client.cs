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

		async Task MainAsync(string[] args) {
			new ClientLog();

			Bot.MessageCreated += async Message_Objects => await MessageEvent.MainEvent(Message_Objects);

			await Bot.ConnectAsync();

			await Task.Delay(-1, CancellationToken.None);
		}
	}
}
