using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Language;
using Avespoir.Core.Modules.Utils;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avespoir.Core.Database.DatabaseMethods;

namespace Avespoir.Core.Modules.Commands.ModeratorCommands {

	[Command("db-usersubmit", RoleLevel.Moderator)]
	class DBUserSubmit : CommandAbstruct {

		internal override LanguageDictionary Description => new LanguageDictionary("Userデータベースにユーザーを追加します") {
			{ Database.Enums.Language.en_US, "Add a user to the User database" }
		};

		internal override LanguageDictionary Usage => new LanguageDictionary("{0}db-usersubmit [名前] [ユーザーID] [役職登録番号]") {
			{ Database.Enums.Language.en_US, "{0}db-usersubmit [Name] [UserID] [Role Number]" }
		};

		internal override async Task Execute(CommandObject Command_Object) {
			try {
				ulong Log_ChannelID = GuildConfigMethods.LogChannelFind(Command_Object.Guild.Id);
				if (Log_ChannelID == 0) {
					await Command_Object.Channel.SendMessageAsync("ログチャンネルが登録されてません");
					return;
				}
				DiscordChannel Log_Channel = Command_Object.Guild.GetChannel(Log_ChannelID);

				string[] msgs = Command_Object.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyText);
					return;
				}

				string msgs_Name;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyName);
					return;
				}
				msgs_Name = msgs[0];

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[1], out ulong msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[2], out uint msgs_RoleNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotNumber);
					return;
				}

				if (AllowUsersMethods.AllowUserExist(Command_Object.Guild.Id, msgs_Name)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.NameRegisted);
					return;
				}

				if (AllowUsersMethods.AllowUserExist(Command_Object.Guild.Id, msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdRegisted);
					return;
				}

				if (PendingUsersMethods.PendingUserExist(Command_Object.Guild.Id, msgs_Name)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.NameRegisted);
					return;
				}

				if (PendingUsersMethods.PendingUserExist(Command_Object.Guild.Id, msgs_ID)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.IdRegisted);
					return;
				}

				if (!RolesMethods.RoleFind(Command_Object.Guild.Id, msgs_RoleNum, out Roles DBRolesNum)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.RoleNumberNotFound);
					return;
				}

				if (!await Authentication.Confirmation(Command_Object)) {
					await Command_Object.Channel.SendMessageAsync(Command_Object.Language.AuthFailure);
					return;
				}

				DiscordEmbedBuilder SubmitEmbed = new DiscordEmbedBuilder();

				DiscordRole GuildRole = Command_Object.Guild.GetRole(DBRolesNum.Uuid);
				SubmitEmbed
					.WithTitle("ユーザー登録の申請がされました")
					.WithDescription("このユーザーを入れることを許可しない場合は**7日後までに**下のリアクションを押してください！")
					.AddField(
						"登録情報",
						string.Format("名前; {0}\nUuid: {1}\nRole: {2}", msgs_Name, msgs_ID, GuildRole.Name)
					)
					.WithColor(new DiscordColor(0x1971FF))
					.WithTimestamp(DateTime.Now)
					.WithFooter(string.Format("{0} Bot", Client.Bot.CurrentUser.Username));
				DiscordMessage EmbedMessage = await Log_Channel.SendMessageAsync(embed: SubmitEmbed.Build());
				await EmbedMessage.CreateReactionAsync(DiscordEmoji.FromUnicode("❌"));

				PendingUsersMethods.PendingUserInsert(Command_Object.Guild.Id, msgs_ID, msgs_Name, msgs_RoleNum, EmbedMessage.Id);

				await Command_Object.Channel.SendMessageAsync("申請が完了しました！詳細はサーバーログを確認してください！");
			}
			catch (IndexOutOfRangeException) {
				await Command_Object.Channel.SendMessageAsync(Command_Object.Language.TypingMissed);
			}
		}
	}
}
