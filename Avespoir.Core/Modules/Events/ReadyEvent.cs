using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ReadyEvent {

		internal static bool ExitCheck = false;

		private static readonly DiscordActivity StartingAct = new DiscordActivity("Starting...", ActivityType.Playing);

		private static DiscordActivity ReadyAct => new DiscordActivity(CommandConfig.Prefix + "help", ActivityType.Playing);

		internal static Task Main(DiscordClient Bot, ReadyEventArgs _) {
			Log.Debug("ReadyEvent " + "Start...");

			Log.Debug(string.Format("ShardCount: {0}, ShardId: {1}", Bot.ShardCount, Bot.ShardId));

			Log.Debug($"Logged in Shards Count: {Bot.ShardId}");
			if (Bot.ShardId == 0) {
				Bot.UpdateStatusAsync(StartingAct, UserStatus.DoNotDisturb).ConfigureAwait(false);

				/*foreach (DiscordGuild Guild in Bot.Guilds.Values) {
					foreach (SocketVoiceChannel VoiceChannel in Guild.VoiceStates.) {
						Log.Debug($"UserCount: {VoiceChannel.Users.Count}");
						foreach (SocketGuildUser User in VoiceChannel.Users) {
							if (User.Id == Bot.CurrentUser.Id) {
								Log.Info("Found Bot Connection");
								VoiceChannel.ConnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
								VoiceChannel.DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
							}
						}
					}
				}*/
			}

			Task.Run(() => StartStatus()).ConfigureAwait(false);
			//Task.Run(() => StartVCCheck()).ConfigureAwait(false);

			//StartStatus().ConfigureAwait(false);
			//StartVCCheck().ConfigureAwait(false);
			Log.Info($"{Bot.CurrentUser.Username}(ShardID: {Bot.ShardId}) Bot Ready!");
			Log.Debug("ReadyEvent " + "End...");
			return Task.CompletedTask;
		}

		static async Task StartStatus() {
			try {
				while (!ExitCheck) {
					await Client.Bot.UpdateStatusAsync(ReadyAct, UserStatus.Online).ConfigureAwait(false);

					await Task.Delay(1000).ConfigureAwait(false);
				}
			}
			catch (Exception Error) {
				Log.Error("Startstatus Error", Error);
			}
		}

		/*
		static async Task StartVCCheck() {
			try {
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
			catch (Exception Error) {
				Log.Error("StartVCCheck Error", Error);
			}
		}
		*/
	}
}
