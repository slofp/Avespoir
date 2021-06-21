using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Audio;
using Avespoir.Core.Modules.Logger;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ReadyEvent {

		internal static bool ExitCheck = false;

		internal static Task Main(DiscordSocketClient Bot) {
			Log.Debug("ReadyEvent " + "Start...");

			Log.Debug($"Logged in Shards Count: {Client.Bot.Shards.Count}");
			if (Client.Bot.Shards.Count == 1) {
				Bot.SetGameAsync("Starting...").ConfigureAwait(false);
				Bot.SetStatusAsync(UserStatus.DoNotDisturb).ConfigureAwait(false);

				foreach (SocketGuild Guild in Bot.Guilds) {
					foreach (SocketVoiceChannel VoiceChannel in Guild.VoiceChannels) {
						Log.Debug($"UserCount: {VoiceChannel.Users.Count}");
						foreach (SocketGuildUser User in VoiceChannel.Users) {
							if (User.Id == Bot.CurrentUser.Id) {
								Log.Info("Found Bot Connection");
								VoiceChannel.ConnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
								VoiceChannel.DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
							}
						}
					}
				}
			}

			Log.Info($"{Bot.CurrentUser.Username}(ShardID: {Bot.ShardId}) Bot Ready!");

			StartStatus().ConfigureAwait(false);
			StartVCCheck().ConfigureAwait(false);
			Log.Debug("ReadyEvent " + "End...");
			return Task.CompletedTask;
		}

		static async Task StartStatus() {
			while (!ExitCheck) {
				await Client.Bot.SetGameAsync(CommandConfig.Prefix + "help").ConfigureAwait(false);
				await Client.Bot.SetStatusAsync(UserStatus.Online).ConfigureAwait(false);

				await Task.Delay(1000).ConfigureAwait(false);
			}
		}

		static async Task StartVCCheck() {
			while (!ExitCheck) {
				if (Client.ConnectedVoiceChannel_Dict.Count == 0) continue;

				foreach (KeyValuePair<ulong, VCInfo> ConnectedVoiceChannel in Client.ConnectedVoiceChannel_Dict) {
					if (DateTime.Now - ConnectedVoiceChannel.Value.LastUpdateDate >= TimeSpan.FromMinutes(10)) {
						await ConnectedVoiceChannel.Value.Finalize().ConfigureAwait(false);

						Log.Info("Disconnected from VC after 10 minutes");
					}
				}
			}
		}
	}
}
