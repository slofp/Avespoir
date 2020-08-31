using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("help")]
		public async Task Help(CommandObjects CommandObject) {
			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

			await CommandObject.Channel.SendMessageAsync(string.Format(CommandObject.Language.DMMention, CommandObject.Member.Mention));

			string GuildPublicPrefix = await DatabaseMethods.PublicPrefixFind(CommandObject.Guild.Id).ConfigureAwait(false);
			if (GuildPublicPrefix == null) GuildPublicPrefix = CommandConfig.PublicPrefix;

			DiscordEmbed PublicEmbed = new DiscordEmbedBuilder()
				.WithTitle(CommandObject.Language.HelpPublicCommand)
				.WithDescription(string.Format(CommandObject.Language.HelpCommandPrefix, GuildPublicPrefix))
				.AddField(
					CommandObject.Language.HelpPing,
					"`" + GuildPublicPrefix + "ping" + "`"
				)
				.AddField(
					CommandObject.Language.HelpVer,
					"`" + GuildPublicPrefix + "ver" + "`"
				)
				.AddField(
					CommandObject.Language.HelpHelp,
					"`" + GuildPublicPrefix + "help" + "`"
				)
				.AddField(
					CommandObject.Language.HelpFind,
					"`" + GuildPublicPrefix + "find" + " " + CommandObject.Language.HelpFindUserId + "`"
				)
				.AddField(
					CommandObject.Language.HelpEmoji,
					"`" + GuildPublicPrefix + "emoji" + " " + CommandObject.Language.HelpEmojiName + " " + CommandObject.Language.HelpEmojiImage + "`"
				)
				.AddField(
					CommandObject.Language.HelpInvite,
					"`" + GuildPublicPrefix + "invite" + " " + CommandObject.Language.HelpInviteChannel + "`"
				)
				.AddField(
					CommandObject.Language.HelpStatus,
					"`" + GuildPublicPrefix + "status" + " " + CommandObject.Language.HelpStatusMention + "`"
				)
				.AddField(
					CommandObject.Language.HelpRoll,
					"`" + GuildPublicPrefix + "roll" + " " + CommandObject.Language.HelpRollMaxvalue + " " + CommandObject.Language.HelpRollMinvalue + "`"
				)
				.WithColor(new DiscordColor(0x00B06B))
				.WithTimestamp(DateTime.Now)
				.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
			await CommandObject.Member.SendMessageAsync(default, default, PublicEmbed);

			try {
				RoleLevel DBRoleLevel;
				if (CommandObject.Message.Author.Id == CommandObject.Guild.Owner.Id) DBRoleLevel = RoleLevel.Moderator;
				else {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, CommandObject.Message.Author.Id);
					AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

					FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUsersID.RoleNum);
					Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

					DBRoleLevel = (RoleLevel) Enum.Parse(typeof(RoleLevel), DBRolesNum.RoleLevel);
				}

				string GuildModeratorPrefix = await DatabaseMethods.ModeratorPrefixFind(CommandObject.Guild.Id).ConfigureAwait(false);
				if (GuildModeratorPrefix == null) GuildModeratorPrefix = CommandConfig.ModeratorPrefix;

				if (DBRoleLevel == RoleLevel.Moderator) { // CommandObject.Message.Author.Id == CommandObject.Guild.Owner.Id
					DiscordEmbed ModeratorEmbed = new DiscordEmbedBuilder()
						.WithTitle(CommandObject.Language.HelpModeratorCommand)
						.WithDescription(string.Format(CommandObject.Language.HelpCommandPrefix, GuildModeratorPrefix))
						.AddField(
							CommandObject.Language.HelpDBUserList,
							"`" + GuildModeratorPrefix + "db-userlist" + "`"
						)
						.AddField(
							CommandObject.Language.HelpDBRoleList,
							"`" + GuildModeratorPrefix + "db-rolelist" + "`"
						)
						.AddField(
							CommandObject.Language.HelpDBUserAdd,
							"`" + GuildModeratorPrefix + "db-useradd" + " " + CommandObject.Language.HelpDBUserAddName + " " + CommandObject.Language.HelpDBUserAddUserId + " " + CommandObject.Language.HelpDBUserAddRoleNum + "`"
						)
						.AddField(
							CommandObject.Language.HelpDBRoleAdd,
							"`" + GuildModeratorPrefix + "db-roleadd" + " " + CommandObject.Language.HelpDBRoleAddRoleId + " " + CommandObject.Language.HelpDBRoleAddRoleNum + " " + CommandObject.Language.HelpDBRoleAddRoleType + "`"
						)
						.AddField(
							CommandObject.Language.HelpDBUserChangeRole,
							"`" + GuildModeratorPrefix + "db-usercrole" + " " + CommandObject.Language.HelpDBUserChangeRoleUserId + " " + CommandObject.Language.HelpDBUserChangeRoleRoleNum + "`"
						)
						.AddField(
							CommandObject.Language.HelpDBUserDelete,
							"`" + GuildModeratorPrefix + "db-userdel" + " " + CommandObject.Language.HelpDBUserDeleteUserId + "`"
						)
						.AddField(
							CommandObject.Language.HelpDBRoleDelete,
							"`" + GuildModeratorPrefix + "db-roledel" + " " + CommandObject.Language.HelpDBRoleDeleteRoleId + "`"
						)
						.AddField(
							CommandObject.Language.HelpConfig,
							"`" + GuildModeratorPrefix + "config" + " " + CommandObject.Language.HelpConfigFirstArgs + " " + CommandObject.Language.HelpConfigValue + "`"
						)
						.AddField(
							CommandObject.Language.HelpConfigArgs,
							"`" + "whitelist" + " | " + "publicprefix" + " | " + "moderatorprefix" + " | " + "logchannel" + " | " + "language" + "`"
						)
						.WithColor(new DiscordColor(0xF6AA00))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
					await CommandObject.Member.SendMessageAsync(default, default, ModeratorEmbed);
				}
			}
			catch (InvalidOperationException) {
				Log.Error("Invalid database element");
			}

			if (CommandObject.Message.Author.Id == ClientConfig.BotownerId) {
				DiscordEmbed BotownerEmbed = new DiscordEmbedBuilder()
					.WithTitle("Botowner Commands")
					.WithDescription(string.Format("Prefix is {0}", CommandConfig.BotownerPrefix))
					.AddField(
						"Restart This Bot",
						"`" + CommandConfig.BotownerPrefix + "restart" + "`"
					)
					.AddField(
						"Logout This Bot",
						"`" + CommandConfig.BotownerPrefix + "logout" + "`"
					)
					.WithColor(new DiscordColor(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
				await CommandObject.Member.SendMessageAsync(default, default, BotownerEmbed);
			}
		}
	}
}
