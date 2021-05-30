using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-usercrole", RoleLevel.Moderator)]
	class DBUserChangeRole : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Userデータベースに登録されているユーザーの役職を変更します") {
			{ Database.Enums.Language.en_US, "Change the Role of a user in the User database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-usercrole [ユーザーID] [役職登録番号]") {
			{ Database.Enums.Language.en_US, "{0}db-usercrole [UserID] [Role number]" }
		};

		internal override async Task Execute(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out ulong msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[1], out uint msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotNumber);
					return;
				}

				if (!Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(CommandObject.Guild.Id, msgs_ID, out AllowUsers DBAllowUsersID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdNotRegisted);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(CommandObject.Guild.Id, msgs_RoleNum, out Roles DBRolesNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotFound);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(CommandObject.Guild.Id, DBAllowUsersID.RoleNum, out Roles DBBeforeRolesNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.BeforeRoleNotFound);
					return;
				}

				DBAllowUsersID.RoleNum = msgs_RoleNum;
				Database.DatabaseMethods.AllowUsersMethods.AllowUserUpdate(DBAllowUsersID);

				DiscordMember GuildMember = await CommandObject.Guild.GetMemberAsync(msgs_ID);

				DiscordRole GuildAfterRole = CommandObject.Guild.GetRole(DBRolesNum.Uuid);
				await GuildMember.GrantRoleAsync(GuildAfterRole);

				DiscordRole GuildBeforeRole = CommandObject.Guild.GetRole(DBBeforeRolesNum.Uuid);
				await GuildMember.RevokeRoleAsync(GuildBeforeRole);

				string ResultText = string.Format(CommandObject.Language.DBUserChangeRoleSuccess, GuildMember.Username + "#" + GuildMember.Discriminator, GuildBeforeRole.Name, GuildAfterRole.Name);
				await CommandObject.Message.Channel.SendMessageAsync(ResultText);

				RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRolesNum.RoleLevel);
				bool GuildLeaveBan = Database.DatabaseMethods.GuildConfigMethods.LeaveBanFind(CommandObject.Guild.Id);
				if (GuildLeaveBan) {
					if (DBRoleLevel == RoleLevel.Public) {
						DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
							.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								CommandObject.Language.DMEmbed_Public1,
								CommandObject.Language.DMEmbed_Public2
							)
							.AddField(
								CommandObject.Language.DMEmbed_Public3,
								CommandObject.Language.DMEmbed_Public4
							)
							.AddField(
								CommandObject.Language.DMEmbed_LeaveBan1,
								string.Format(CommandObject.Language.DMEmbed_LeaveBan2, GuildAfterRole.Name)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(WelcomeEmbed);

						return;
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
							.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								CommandObject.Language.DMEmbed_Moderator1,
								CommandObject.Language.DMEmbed_Moderator2
							)
							.AddField(
								CommandObject.Language.DMEmbed_Moderator3,
								string.Format(CommandObject.Language.DMEmbed_Moderator4, CommandObject.Client.CurrentUser.Username)
							)
							.AddField(
								CommandObject.Language.DMEmbed_Moderator5,
								CommandObject.Language.DMEmbed_Moderator6
							)
							.AddField(
								CommandObject.Language.DMEmbed_LeaveBan1,
								string.Format(CommandObject.Language.DMEmbed_LeaveBan2, GuildAfterRole.Name)
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(WelcomeEmbed);

						return;
					}
				}
				else {
					if (DBRoleLevel == RoleLevel.Public) {
						DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
							.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								CommandObject.Language.DMEmbed_Public1,
								CommandObject.Language.DMEmbed_Public2
							)
							.AddField(
								CommandObject.Language.DMEmbed_Public3,
								CommandObject.Language.DMEmbed_Public4
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(WelcomeEmbed);

						return;
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
							.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								CommandObject.Language.DMEmbed_Moderator1,
								CommandObject.Language.DMEmbed_Moderator2
							)
							.AddField(
								CommandObject.Language.DMEmbed_Moderator3,
								string.Format(CommandObject.Language.DMEmbed_Moderator4, CommandObject.Client.CurrentUser.Username)
							)
							.AddField(
								CommandObject.Language.DMEmbed_Moderator5,
								CommandObject.Language.DMEmbed_Moderator6
							)
							.WithColor(new DiscordColor(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(WelcomeEmbed);

						return;
					}
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
				return;
			}
		}
	}
}
