using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Avespoir.Core.Database.DatabaseMethods;

namespace Avespoir.Core.Modules.Events {

	class GuildMemberAddEvent {

		private static async Task BotProcess(GuildMemberAddEventArgs MemberObjects, GetLanguage Get_Language) {
			if (RolesMethods.RoleFind(MemberObjects.Guild.Id, RoleLevel.Bot, out Roles DBRoleRoleLevel)) {
				DiscordRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleLevel.Uuid);
				await MemberObjects.Member.GrantRoleAsync(GuildRole);
			}
			else Log.Warning("Could not grant role");

			ulong Guild_ChannelID = GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (Guild_ChannelID != 0) {
				DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
				DiscordEmbed​Builder LogChannelEmbed = new DiscordEmbed​Builder()
					.WithTitle(Get_Language.Language_Data.IsBot)
					.WithDescription(
						string.Format(
							Get_Language.Language_Data.Bot_BanDescription,
							MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
							MemberObjects.Member.Id
						)
					)
					.WithColor(new DiscordColor(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(
						string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
					)
					.WithAuthor(Get_Language.Language_Data.Accessed);
				await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
			}
			else Log.Warning("Could not send from log channel");

			Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is Bot");
		}

		private static async Task AllowUserProcess(GuildMemberAddEventArgs MemberObjects, GetLanguage Get_Language, AllowUsers DBAllowUserID) {
			if (RolesMethods.RoleFind(MemberObjects.Guild.Id, DBAllowUserID.RoleNum, out Roles DBRoleRoleNum)) {
				DiscordRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleNum.Uuid);

				ulong Guild_ChannelID = GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

				if (Guild_ChannelID != 0) {
					DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
					DiscordEmbed​Builder LogChannelEmbed = new DiscordEmbed​Builder()
						.WithTitle(Get_Language.Language_Data.JoinPass)
						.WithDescription(
							string.Format(
								Get_Language.Language_Data.UserDescription,
								MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
								MemberObjects.Member.Id,
								GuildRole.Name
							)
						)
						.WithColor(new DiscordColor(0x00B06B))
						.WithTimestamp(DateTime.Now)
						.WithFooter(
							string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
						)
						.WithAuthor(Get_Language.Language_Data.AccessPermission);
					await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
				}
				else Log.Warning("Could not send from log channel");

				RoleLevel DBRoleLevel = DBRoleRoleNum.RoleLevel;
				bool GuildLeaveBan = GuildConfigMethods.LeaveBanFind(MemberObjects.Guild.Id);
				if (GuildLeaveBan) {
					if (DBRoleLevel == RoleLevel.Public) {
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
						DiscordEmbed​Builder WelcomeEmbed = new DiscordEmbed​Builder()
							.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								Get_Language.Language_Data.DMEmbed_Public1,
								Get_Language.Language_Data.DMEmbed_Public2
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Public3,
								Get_Language.Language_Data.DMEmbed_Public4
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_LeaveBan1,
								string.Format(Get_Language.Language_Data.DMEmbed_LeaveBan2, GuildRole.Name)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.Member.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
						DiscordEmbed​Builder WelcomeEmbed = new DiscordEmbed​Builder()
							.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								Get_Language.Language_Data.DMEmbed_Moderator1,
								Get_Language.Language_Data.DMEmbed_Moderator2
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Moderator3,
								string.Format(Get_Language.Language_Data.DMEmbed_Moderator4, Client.Bot.CurrentUser.Username)
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Moderator5,
								Get_Language.Language_Data.DMEmbed_Moderator6
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_LeaveBan1,
								string.Format(Get_Language.Language_Data.DMEmbed_LeaveBan2, GuildRole.Name)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.Member.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
				}
				else {
					if (DBRoleLevel == RoleLevel.Public) {
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
						DiscordEmbed​Builder WelcomeEmbed = new DiscordEmbed​Builder()
							.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								Get_Language.Language_Data.DMEmbed_Public1,
								Get_Language.Language_Data.DMEmbed_Public2
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Public3,
								Get_Language.Language_Data.DMEmbed_Public4
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.Member.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
						DiscordEmbed​Builder WelcomeEmbed = new DiscordEmbed​Builder()
							.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								Get_Language.Language_Data.DMEmbed_Moderator1,
								Get_Language.Language_Data.DMEmbed_Moderator2
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Moderator3,
								string.Format(Get_Language.Language_Data.DMEmbed_Moderator4, Client.Bot.CurrentUser.Username)
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Moderator5,
								Get_Language.Language_Data.DMEmbed_Moderator6
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.Member.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
				}

				Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is allowed join");
			}
			else await MemberObjects.Member.SendMessageAsync(Get_Language.Language_Data.RoleNumNullMessage);
		}

		private static async Task DenyUserProcess(GuildMemberAddEventArgs MemberObjects, GetLanguage Get_Language) {
			await MemberObjects.Member.SendMessageAsync(Get_Language.Language_Data.PermissionDenied);
			await MemberObjects.Member.RemoveAsync(Get_Language.Language_Data.NotAccessed);

			ulong Guild_ChannelID = GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (Guild_ChannelID != 0) {
				DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
				DiscordEmbed​Builder LogChannelEmbed = new DiscordEmbed​Builder()
					.WithTitle(Get_Language.Language_Data.AccessDenied)
					.WithDescription(
						string.Format(
							Get_Language.Language_Data.Bot_BanDescription,
							MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator,
							MemberObjects.Member.Id
						)
					)
					.WithColor(new DiscordColor(0xFF4B00))
					.WithTimestamp(DateTime.Now)
					.WithFooter(
						string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
					)
					.WithAuthor(Get_Language.Language_Data.NotAccessed);
				await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
			}
			else Log.Warning("Could not send from log channel");

			Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is not allowed join");
		}

		private static async Task Fork(GuildMemberAddEventArgs MemberObjects, GetLanguage Get_Language) {
			if (!GuildConfigMethods.WhitelistFind(MemberObjects.Guild.Id)) {
				Log.Info("Whitelist Disabled");
				return;
			}
			if (MemberObjects.Member.IsBot) {
				await BotProcess(MemberObjects, Get_Language).ConfigureAwait(false);
				return;
			}

			if (AllowUsersMethods.AllowUserFind(MemberObjects.Guild.Id, MemberObjects.Member.Id, out AllowUsers DBAllowUserID)) {
				await AllowUserProcess(MemberObjects, Get_Language, DBAllowUserID).ConfigureAwait(false);
				return;
			}
			else { // if DBAllowUserID is null, processes will not be executed from here. And kick.
				await DenyUserProcess(MemberObjects, Get_Language).ConfigureAwait(false);
				return;
			}
		}

		internal static async Task Main(DiscordClient Bot, GuildMemberAddEventArgs MemberObjects) {
			Log.Debug("GuildMemberAddEvent " + "Start...");

			GetLanguage Get_Language = new GetLanguage(GuildConfigMethods.LanguageFind(MemberObjects.Guild.Id));

			await Fork(MemberObjects, Get_Language).ConfigureAwait(false);

			Log.Debug("GuildMemberAddEvent " + "End...");
		}
	}
}
