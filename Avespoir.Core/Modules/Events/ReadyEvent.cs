﻿using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Database.DatabaseMethods;
using DSharpPlus.VoiceNext;

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

				/* 自動切断できない...
				foreach (ulong GuildID in Bot.Guilds.Keys) {
					Log.Debug($"Guildid: {GuildID}");

					DiscordGuild Guild = Bot.GetGuildAsync(GuildID).ConfigureAwait(false).GetAwaiter().GetResult();
					Log.Debug($"Guild: {Guild.Name}");

					IReadOnlyCollection<DiscordMember> Guild_Members =
						Guild.GetAllMembersAsync().ConfigureAwait(false).GetAwaiter().GetResult();

					foreach (DiscordChannel Channel in Guild.GetChannelsAsync().ConfigureAwait(false).GetAwaiter().GetResult()) {
						Log.Debug($"Channel: {Channel.Name}, {Channel.Type}");

						if (Channel.Type == ChannelType.Voice) {
							List<DiscordMember> Channel_Users =
								(from x in Guild_Members
								 where x.VoiceState?.Channel.Id == Channel.Id
								 select x).ToList();

							Log.Debug($"VC Member (Count: {Channel_Users.Count})");
							foreach (DiscordMember VCinMember in Channel_Users) {
								Log.Debug($"  {VCinMember.DisplayName}");

								if (VCinMember.Id == Bot.CurrentUser.Id) {
									Log.Info("found VC with bot in it!");
									Log.Info("Connect and Disconnecting...");

									Channel.ConnectAsync().ConfigureAwait(false).GetAwaiter().GetResult().Disconnect();

									Log.Info("Disconnected!");
								}
							}
						}
					}
				}*/
			}

			Task.Run(() => StartStatus()).ConfigureAwait(false);
			Task.Run(() => StartPendingConfirm()).ConfigureAwait(false);
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

		static async Task StartPendingConfirm() {
			try {
				while (!ExitCheck) {
					List<PendingUsers> PendingUserList = PendingUsersMethods.PendingUserList();
					for (int i = 0; i < PendingUserList.Count; i++) {
						PendingUsers PendingUser = PendingUserList[i];
						if (DateTime.Now > PendingUser.PendingStart.AddDays(7)) {
							AllowUsersMethods.AllowUserInsert(PendingUser.GuildID, PendingUser.Uuid, PendingUser.Name, PendingUser.RoleNum);
							PendingUsersMethods.PendingUserDelete(PendingUser);

							ulong Log_ChannelID = GuildConfigMethods.LogChannelFind(PendingUser.GuildID);
							if (Log_ChannelID == 0) {
								Log.Error("Not found LogChannel");
								return;
							}
							DiscordGuild Guild = await Client.Bot.GetGuildAsync(PendingUser.GuildID).ConfigureAwait(false);
							DiscordChannel Log_Channel = Guild.GetChannel(Log_ChannelID);

							DiscordEmbedBuilder SubmitEmbed = new DiscordEmbedBuilder();

							SubmitEmbed
								.WithTitle("ユーザー登録の申請が受理されました")
								.WithDescription("申請が受理され正式に登録されました")
								.WithColor(new DiscordColor(0x00B06B))
								.WithTimestamp(DateTime.Now)
								.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
							await Log_Channel.SendMessageAsync(embed: SubmitEmbed.Build()).ConfigureAwait(false);
						}
					}

					await Task.Delay(1000).ConfigureAwait(false);
				}
			}
			catch (Exception Error) {
				Log.Error("StartPendingConfirm Error", Error);
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
