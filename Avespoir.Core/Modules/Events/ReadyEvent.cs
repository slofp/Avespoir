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

			Task.Run(() => StartStatus()).ConfigureAwait(false);
			Task.Run(() => StartVCCheck()).ConfigureAwait(false);
			//Task.Run(() => AutoReconnect()).ConfigureAwait(false);

			//StartStatus().ConfigureAwait(false);
			//StartVCCheck().ConfigureAwait(false);
			//AutoReconnect().ConfigureAwait(false);
			Log.Info($"{Bot.CurrentUser.Username}(ShardID: {Bot.ShardId}) Bot Ready!");
			Log.Debug("ReadyEvent " + "End...");
			return Task.CompletedTask;
		}

		static async Task AutoReconnect() {
			DateTime StartTime = DateTime.Now;

			TimeSpan Min_30 = new TimeSpan(0, 0, 30); // とりあえず30秒
			Log.Info("AutoRestarter Start");
			try {
				while (!ExitCheck) {
					if (DateTime.Now - StartTime >= Min_30) {
						Log.Info("Reconnecting...");
						ExitCheck = true;
						await Task.Delay(1000).ConfigureAwait(false);

						await Client.Bot.StopAsync().ConfigureAwait(false);
						await Client.Bot.LogoutAsync().ConfigureAwait(false);

						ExitCheck = false;

						await Client.Bot.LoginAsync(TokenType.Bot, ClientConfig.Token).ConfigureAwait(false);
						await Client.Bot.StartAsync().ConfigureAwait(false);
						return;
					}

					await Task.Delay(1000).ConfigureAwait(false);
				}
			}
			catch (Exception Error) {
				Log.Error("AutoRestarter Error", Error);
			}

			Log.Info("AutoRestarter Exit");
		}

		static async Task StartStatus() {
			try {
				while (!ExitCheck) {
					await Client.Bot.SetGameAsync(CommandConfig.Prefix + "help").ConfigureAwait(false);
					await Client.Bot.SetStatusAsync(UserStatus.Online).ConfigureAwait(false);

					await Task.Delay(1000).ConfigureAwait(false);
				}
			}
			catch (Exception Error) {
				Log.Error("Startstatus Error", Error);
			}
		}

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
	}
}
