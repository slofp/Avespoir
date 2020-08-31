using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class GuildMemberAddEvent {

		internal static async Task Main(GuildMemberAddEventArgs MemberObjects) {
			Log.Debug("GuildMemberAddEvent " + "Start...");

			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
			FilterDefinition<AllowUsers> DBAllowUsersGuildIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.GuildID, MemberObjects.Guild.Id);
			FilterDefinition<Roles> DBRolesGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, MemberObjects.Guild.Id);

			GetLanguage Get_Language;
			string GuildLanguageString = await DatabaseMethods.LanguageFind(MemberObjects.Guild.Id).ConfigureAwait(false);
			if (GuildLanguageString == null) Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
			else {
				if (!Enum.TryParse(GuildLanguageString, true, out Database.Enums.Language GuildLanguage))
					Get_Language = new GetLanguage(Database.Enums.Language.ja_JP);
				else Get_Language = new GetLanguage(GuildLanguage);
			}

			try {
				if (!await DatabaseMethods.WhitelistFind(MemberObjects.Guild.Id).ConfigureAwait(false)) {
					Log.Info("Whitelist Disabled");

					return;
				};

				if (MemberObjects.Member.IsBot) {
					try {
						FilterDefinition<Roles> DBRolesRoleLevelFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleLevel, Enum.GetName(typeof(RoleLevel), RoleLevel.Bot));
						FilterDefinition<Roles> DBRolesGuildIDRoleLevelFilter = Builders<Roles>.Filter.And(DBRolesGuildIDFilter, DBRolesRoleLevelFilter);
						Roles DBRoleRoleLevel = await (await DBRolesCollection.FindAsync(DBRolesGuildIDRoleLevelFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRoleRoleLevel is null...?

						DiscordRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleLevel.uuid);
						await MemberObjects.Member.GrantRoleAsync(GuildRole);
					}
					catch (InvalidOperationException) {
						Log.Warning("Could not grant role");
					}

					ulong Guild_ChannelID = await DatabaseMethods.LogChannelFind(MemberObjects.Guild.Id).ConfigureAwait(false);

					if (Guild_ChannelID != 0) {
						DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
						DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
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
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							)
							.WithAuthor(Get_Language.Language_Data.Accessed);
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					else Log.Warning("Could not send from log channel");

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is Bot");
					return;
				}

				FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, MemberObjects.Member.Id);
				FilterDefinition<AllowUsers> DBAllowUsersGuildIDIDFilter = Builders<AllowUsers>.Filter.And(DBAllowUsersGuildIDFilter, DBAllowUsersIDFilter);
				AllowUsers DBAllowUserID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
				// if DBAllowUserID is null, processes will not be executed from here. And kick.

				try {
					FilterDefinition<Roles> DBRolesRoleNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUserID.RoleNum);
					FilterDefinition<Roles> DBRolesGuildIDRoleNumFilter = Builders<Roles>.Filter.And(DBRolesGuildIDFilter, DBRolesRoleNumFilter);
					Roles DBRoleRoleNum = await (await DBRolesCollection.FindAsync(DBRolesGuildIDRoleNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBRoleRoleNum is null...?

					DiscordRole GuildRole = MemberObjects.Guild.GetRole(DBRoleRoleNum.uuid);

					ulong Guild_ChannelID = await DatabaseMethods.LogChannelFind(MemberObjects.Guild.Id).ConfigureAwait(false);

					if (Guild_ChannelID != 0) {
						DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
						DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
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
								string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
							)
							.WithAuthor(Get_Language.Language_Data.AccessPermission);
						await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
					}
					else Log.Warning("Could not send from log channel");

					RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRoleRoleNum.RoleLevel);
					bool GuildLeaveBan = await DatabaseMethods.LeaveBanFind(MemberObjects.Guild.Id).ConfigureAwait(false);
					if (GuildLeaveBan) {
						if (DBRoleLevel == RoleLevel.Public) {
							await MemberObjects.Member.GrantRoleAsync(GuildRole);
							DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
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
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								);
							await MemberObjects.Member.SendMessageAsync(default, default, WelcomeEmbed);
						}
						else if (DBRoleLevel == RoleLevel.Moderator) {
							await MemberObjects.Member.GrantRoleAsync(GuildRole);
							DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
								.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
								.AddField(
									Get_Language.Language_Data.DMEmbed_Moderator1,
									Get_Language.Language_Data.DMEmbed_Moderator2
								)
								.AddField(
									Get_Language.Language_Data.DMEmbed_Moderator3,
									string.Format(Get_Language.Language_Data.DMEmbed_Moderator4, MemberObjects.Client.CurrentUser.Username)
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
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								);
							await MemberObjects.Member.SendMessageAsync(default, default, WelcomeEmbed);
						}
					}
					else {
						if (DBRoleLevel == RoleLevel.Public) {
							await MemberObjects.Member.GrantRoleAsync(GuildRole);
							DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
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
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								);
							await MemberObjects.Member.SendMessageAsync(default, default, WelcomeEmbed);
						}
						else if (DBRoleLevel == RoleLevel.Moderator) {
							await MemberObjects.Member.GrantRoleAsync(GuildRole);
							DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
								.WithTitle(string.Format(Get_Language.Language_Data.WelcomeEmbedTitle, MemberObjects.Guild.Name, GuildRole.Name))
								.AddField(
									Get_Language.Language_Data.DMEmbed_Moderator1,
									Get_Language.Language_Data.DMEmbed_Moderator2
								)
								.AddField(
									Get_Language.Language_Data.DMEmbed_Moderator3,
									string.Format(Get_Language.Language_Data.DMEmbed_Moderator4, MemberObjects.Client.CurrentUser.Username)
								)
								.AddField(
									Get_Language.Language_Data.DMEmbed_Moderator5,
									Get_Language.Language_Data.DMEmbed_Moderator6
								)
								.WithColor(new DiscordColor(0x00B06B))
								.WithTimestamp(DateTime.Now)
								.WithFooter(
									string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
								);
							await MemberObjects.Member.SendMessageAsync(default, default, WelcomeEmbed);
						}
					}

					Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is allowed join");
					return;
				}
				catch (InvalidOperationException) {
					await MemberObjects.Member.SendMessageAsync(Get_Language.Language_Data.RoleNumNullMessage);
				}
			}
			catch (InvalidOperationException) {
				await MemberObjects.Member.SendMessageAsync(Get_Language.Language_Data.PermissionDenied);
				await MemberObjects.Member.RemoveAsync(Get_Language.Language_Data.NotAccessed);

				ulong Guild_ChannelID = await DatabaseMethods.LogChannelFind(MemberObjects.Guild.Id).ConfigureAwait(false);

				if (Guild_ChannelID != 0) {
					DiscordChannel GuildLogChannel = MemberObjects.Guild.GetChannel(Guild_ChannelID);
					DiscordEmbed LogChannelEmbed = new Discord​Embed​Builder()
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
							string.Format("{0} Bot", MemberObjects.Client.CurrentUser.Username)
						)
						.WithAuthor(Get_Language.Language_Data.NotAccessed);
					await GuildLogChannel.SendMessageAsync(default, default, LogChannelEmbed);
				}
				else Log.Warning("Could not send from log channel");

				Log.Debug($"{MemberObjects.Member.Username + "#" + MemberObjects.Member.Discriminator} is not allowed join");
				return;
			}
			finally {
				Log.Debug("GuildMemberAddEvent " + "End...");
			}
		}
	}
}
