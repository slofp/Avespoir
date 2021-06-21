using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class GuildMemberAddEvent {

		private static async Task BotProcess(SocketGuildUser MemberObjects, GetLanguage Get_Language) {
			if (Database.DatabaseMethods.RolesMethods.RoleFind(MemberObjects.Guild.Id, RoleLevel.Bot, out Roles DBRoleRoleLevel)) {
				SocketRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleLevel.Uuid);
				await MemberObjects.AddRoleAsync(GuildRole);
			}
			else Log.Warning("Could not grant role");

			ulong Guild_ChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (Guild_ChannelID != 0) {
				SocketTextChannel GuildLogChannel = MemberObjects.Guild.GetTextChannel(Guild_ChannelID);
				EmbedBuilder LogChannelEmbed = new Embed​Builder()
					.WithTitle(Get_Language.Language_Data.IsBot)
					.WithDescription(
						string.Format(
							Get_Language.Language_Data.Bot_BanDescription,
							MemberObjects.Username + "#" + MemberObjects.Discriminator,
							MemberObjects.Id
						)
					)
					.WithColor(new Color(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(
						string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
					)
					.WithAuthor(Get_Language.Language_Data.Accessed);
				await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
			}
			else Log.Warning("Could not send from log channel");

			Log.Debug($"{MemberObjects.Username + "#" + MemberObjects.Discriminator} is Bot");
		}

		private static async Task AllowUserProcess(SocketGuildUser MemberObjects, GetLanguage Get_Language, AllowUsers DBAllowUserID) {
			if (Database.DatabaseMethods.RolesMethods.RoleFind(MemberObjects.Guild.Id, DBAllowUserID.RoleNum, out Roles DBRoleRoleNum)) {
				SocketRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleNum.Uuid);

				ulong Guild_ChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

				if (Guild_ChannelID != 0) {
					SocketTextChannel GuildLogChannel = MemberObjects.Guild.GetTextChannel(Guild_ChannelID);
					EmbedBuilder LogChannelEmbed = new Embed​Builder()
						.WithTitle(Get_Language.Language_Data.JoinPass)
						.WithDescription(
							string.Format(
								Get_Language.Language_Data.UserDescription,
								MemberObjects.Username + "#" + MemberObjects.Discriminator,
								MemberObjects.Id,
								GuildRole.Name
							)
						)
						.WithColor(new Color(0x00B06B))
						.WithTimestamp(DateTime.Now)
						.WithFooter(
							string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
						)
						.WithAuthor(Get_Language.Language_Data.AccessPermission);
					await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
				}
				else Log.Warning("Could not send from log channel");

				RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRoleRoleNum.RoleLevel);
				bool GuildLeaveBan = Database.DatabaseMethods.GuildConfigMethods.LeaveBanFind(MemberObjects.Guild.Id);
				if (GuildLeaveBan) {
					if (DBRoleLevel == RoleLevel.Public) {
						await MemberObjects.AddRoleAsync(GuildRole);
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
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
							.WithColor(new Color(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						await MemberObjects.AddRoleAsync(GuildRole);
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
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
							.WithColor(new Color(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
				}
				else {
					if (DBRoleLevel == RoleLevel.Public) {
						await MemberObjects.AddRoleAsync(GuildRole);
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
							.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
							.AddField(
								Get_Language.Language_Data.DMEmbed_Public1,
								Get_Language.Language_Data.DMEmbed_Public2
							)
							.AddField(
								Get_Language.Language_Data.DMEmbed_Public3,
								Get_Language.Language_Data.DMEmbed_Public4
							)
							.WithColor(new Color(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						await MemberObjects.AddRoleAsync(GuildRole);
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
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
							.WithColor(new Color(0x00B06B))
							.WithTimestamp(DateTime.Now)
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await MemberObjects.SendMessageAsync(embed: WelcomeEmbed.Build());
					}
				}

				Log.Debug($"{MemberObjects.Username + "#" + MemberObjects.Discriminator} is allowed join");
			}
			else await MemberObjects.SendMessageAsync(Get_Language.Language_Data.RoleNumNullMessage);
		}

		private static async Task DenyUserProcess(SocketGuildUser MemberObjects, GetLanguage Get_Language) {
			await MemberObjects.SendMessageAsync(Get_Language.Language_Data.PermissionDenied);
			await MemberObjects.KickAsync(Get_Language.Language_Data.NotAccessed);

			ulong Guild_ChannelID = Database.DatabaseMethods.GuildConfigMethods.LogChannelFind(MemberObjects.Guild.Id);

			if (Guild_ChannelID != 0) {
				SocketTextChannel GuildLogChannel = MemberObjects.Guild.GetTextChannel(Guild_ChannelID);
				EmbedBuilder LogChannelEmbed = new Embed​Builder()
					.WithTitle(Get_Language.Language_Data.AccessDenied)
					.WithDescription(
						string.Format(
							Get_Language.Language_Data.Bot_BanDescription,
							MemberObjects.Username + "#" + MemberObjects.Discriminator,
							MemberObjects.Id
						)
					)
					.WithColor(new Color(0xFF4B00))
					.WithTimestamp(DateTime.Now)
					.WithFooter(
						string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
					)
					.WithAuthor(Get_Language.Language_Data.NotAccessed);
				await GuildLogChannel.SendMessageAsync(embed: LogChannelEmbed.Build());
			}
			else Log.Warning("Could not send from log channel");

			Log.Debug($"{MemberObjects.Username + "#" + MemberObjects.Discriminator} is not allowed join");
		}

		private static async Task Fork(SocketGuildUser MemberObjects, GetLanguage Get_Language) {
			if (!Database.DatabaseMethods.GuildConfigMethods.WhitelistFind(MemberObjects.Guild.Id)) {
				Log.Info("Whitelist Disabled");
				return;
			}
			if (MemberObjects.IsBot) {
				await BotProcess(MemberObjects, Get_Language).ConfigureAwait(false);
				return;
			}

			if (Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(MemberObjects.Guild.Id, MemberObjects.Id, out AllowUsers DBAllowUserID)) {
				await AllowUserProcess(MemberObjects, Get_Language, DBAllowUserID).ConfigureAwait(false);
				return;
			}
			else { // if DBAllowUserID is null, processes will not be executed from here. And kick.
				await DenyUserProcess(MemberObjects, Get_Language).ConfigureAwait(false);
				return;
			}
		}

		internal static async Task Main(SocketGuildUser MemberObjects) {
			Log.Debug("GuildMemberAddEvent " + "Start...");

			GetLanguage Get_Language;
			string GuildLanguageString = Database.DatabaseMethods.GuildConfigMethods.LanguageFind(MemberObjects.Guild.Id);
			if (GuildLanguageString == null) Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
			else {
				if (Enum.TryParse(GuildLanguageString, true, out Database.Enums.Language GuildLanguage))
					Get_Language = new GetLanguage(GuildLanguage);
				else Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
			}

			await Fork(MemberObjects, Get_Language).ConfigureAwait(false);

			Log.Debug("GuildMemberAddEvent " + "End...");
		}
	}
}
