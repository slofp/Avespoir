using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Logger;
using Discord.WebSocket;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ReadyEvent {

		internal static bool ExitCheck = false;

		static readonly DiscordActivity StartingStatus = new DiscordActivity() {
			Name = "Starting...",
		};

		internal static Task Main(DiscordSocketClient Bot) {
			Log.Debug("ReadyEvent " + "Start...");
			//Bot.UpdateStatusAsync(StartingStatus, UserStatus.DoNotDisturb).ConfigureAwait(false);

			Log.Info($"{Bot.CurrentUser.Username} Bot Ready!");

			//StartStatus().ConfigureAwait(false);
			Log.Debug("ReadyEvent " + "End...");
			return Task.CompletedTask;
		}

		internal static Task Main(DiscordClient Bot, ReadyEventArgs _) {
			Log.Debug("ReadyEvent " + "Start...");
			Bot.UpdateStatusAsync(StartingStatus, UserStatus.DoNotDisturb).ConfigureAwait(false);

			Log.Info($"{Bot.CurrentUser.Username} Bot Ready!");

			StartStatus(Bot).ConfigureAwait(false);
			Log.Debug("ReadyEvent " + "End...");
			return Task.CompletedTask;
		}

		static async Task StartStatus(DiscordClient Bot) {
			while (!ExitCheck) {
				DiscordActivity ReadyStatus = new DiscordActivity() {
					Name = CommandConfig.Prefix + "help",
				};
				
				await Bot.UpdateStatusAsync(ReadyStatus, UserStatus.Online).ConfigureAwait(false);

				await Task.Delay(1000).ConfigureAwait(false);
			}
		}
	}
}
