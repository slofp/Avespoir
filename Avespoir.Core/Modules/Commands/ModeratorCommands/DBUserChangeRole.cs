using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using Discord;
using Discord.WebSocket;
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

		internal override async Task Execute(CommandObject Command_Object) {
			try {
				string[] msgs = Command_Object.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out ulong msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[1], out uint msgs_RoleNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotNumber);
					return;
				}

				if (!Database.DatabaseMethods.AllowUsersMethods.AllowUserFind(Command_Object.Guild.Id, msgs_ID, out AllowUsers DBAllowUsersID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdNotRegisted);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(Command_Object.Guild.Id, msgs_RoleNum, out Roles DBRolesNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotFound);
					return;
				}

				if (!Database.DatabaseMethods.RolesMethods.RoleFind(Command_Object.Guild.Id, DBAllowUsersID.RoleNum, out Roles DBBeforeRolesNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.BeforeRoleNotFound);
					return;
				}

				DBAllowUsersID.RoleNum = msgs_RoleNum;
				Database.DatabaseMethods.AllowUsersMethods.AllowUserUpdate(DBAllowUsersID);

				SocketGuildUser GuildMember = Command_Object.Guild.GetUser(msgs_ID);

				SocketRole GuildAfterRole = Command_Object.Guild.GetRole(DBRolesNum.Uuid);
				await GuildMember.AddRoleAsync(GuildAfterRole);

				SocketRole GuildBeforeRole = Command_Object.Guild.GetRole(DBBeforeRolesNum.Uuid);
				await GuildMember.RemoveRoleAsync(GuildBeforeRole);

				string ResultText = string.Format(Command_Object.Language.DBUserChangeRoleSuccess, GuildMember.Username + "#" + GuildMember.Discriminator, GuildBeforeRole.Name, GuildAfterRole.Name);
				await Command_Object.Channel.SendMessageAsync(ResultText);

				RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRolesNum.RoleLevel);
				bool GuildLeaveBan = Database.DatabaseMethods.GuildConfigMethods.LeaveBanFind(Command_Object.Guild.Id);
				if (GuildLeaveBan) {
					if (DBRoleLevel == RoleLevel.Public) {
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
							.WithTitle(string.Format(Command_Object.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								Command_Object.Language.DMEmbed_Public1,
								Command_Object.Language.DMEmbed_Public2
							)
							.AddField(
								Command_Object.Language.DMEmbed_Public3,
								Command_Object.Language.DMEmbed_Public4
							)
							.AddField(
								Command_Object.Language.DMEmbed_LeaveBan1,
								string.Format(Command_Object.Language.DMEmbed_LeaveBan2, GuildAfterRole.Name)
							)
							.WithColor(new Color(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(embed: WelcomeEmbed.Build());

						return;
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
							.WithTitle(string.Format(Command_Object.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								Command_Object.Language.DMEmbed_Moderator1,
								Command_Object.Language.DMEmbed_Moderator2
							)
							.AddField(
								Command_Object.Language.DMEmbed_Moderator3,
								string.Format(Command_Object.Language.DMEmbed_Moderator4, Client.Bot.CurrentUser.Username)
							)
							.AddField(
								Command_Object.Language.DMEmbed_Moderator5,
								Command_Object.Language.DMEmbed_Moderator6
							)
							.AddField(
								Command_Object.Language.DMEmbed_LeaveBan1,
								string.Format(Command_Object.Language.DMEmbed_LeaveBan2, GuildAfterRole.Name)
							)
							.WithColor(new Color(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(embed: WelcomeEmbed.Build());

						return;
					}
				}
				else {
					if (DBRoleLevel == RoleLevel.Public) {
						EmbedBuilder WelcomeEmbed = new EmbedBuilder()
							.WithTitle(string.Format(Command_Object.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								Command_Object.Language.DMEmbed_Public1,
								Command_Object.Language.DMEmbed_Public2
							)
							.AddField(
								Command_Object.Language.DMEmbed_Public3,
								Command_Object.Language.DMEmbed_Public4
							)
							.WithColor(new Color(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(embed: WelcomeEmbed.Build());

						return;
					}
					else if (DBRoleLevel == RoleLevel.Moderator) {
						EmbedBuilder WelcomeEmbed = new Embed​Builder()
							.WithTitle(string.Format(Command_Object.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
							.AddField(
								Command_Object.Language.DMEmbed_Moderator1,
								Command_Object.Language.DMEmbed_Moderator2
							)
							.AddField(
								Command_Object.Language.DMEmbed_Moderator3,
								string.Format(Command_Object.Language.DMEmbed_Moderator4, Client.Bot.CurrentUser.Username)
							)
							.AddField(
								Command_Object.Language.DMEmbed_Moderator5,
								Command_Object.Language.DMEmbed_Moderator6
							)
							.WithColor(new Color(0x00B06B))
							.WithFooter(
								string.Format("{0} Bot", Client.Bot.CurrentUser.Username)
							);
						await GuildMember.SendMessageAsync(embed: WelcomeEmbed.Build());

						return;
					}
				}
			}
			catch (IndexOutOfRangeException) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.TypingMissed);
				return;
			}
		}
	}
}
