using AvespoirTest.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class ReadyEvent {

		static DiscordGame StartingStatus = new DiscordGame() {
			Name = "Starting...",
		};

		static DiscordGame ReadyStatus = new DiscordGame() {
			Name = "Bot Ready!",
		};

		internal static Task Main(ReadyEventArgs ReadyEventObjects) {
			new DebugLog("ReadyEvent " + "Start...");
			BaseDiscordClient ClientBase = ReadyEventObjects.Client;
			Client.Bot.UpdateStatusAsync(StartingStatus, UserStatus.DoNotDisturb).ConfigureAwait(false);

			Client.Bot.UpdateStatusAsync(ReadyStatus, UserStatus.Online).ConfigureAwait(false);

			new InfoLog($"{ClientBase.CurrentUser.Username} Bot Ready!");
			Task.Delay(5000).ConfigureAwait(false);

			Client.Bot.UpdateStatusAsync(null, UserStatus.Online).ConfigureAwait(false);
			new DebugLog("ReadyEvent " + "End...");

			return Task.CompletedTask;
		}
	}
}
