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

			await CommandObject.Channel.SendMessageAsync($"{CommandObject.Member.Mention} DMをご確認ください！");

			DiscordEmbed PublicEmbed = new DiscordEmbedBuilder()
				.WithTitle("一般コマンド")
				.WithDescription(string.Format("プレフィックスは {0} です", CommandConfig.PublicPrefix))
				.AddField(
					"Pingを測ります",
					"`" + CommandConfig.PublicPrefix + "ping" + "`"
				)
				.AddField(
					"Botのバージョンを表示します",
					"`" + CommandConfig.PublicPrefix + "ver" + "`"
				)
				.AddField(
					"コマンド一覧を表示します",
					"`" + CommandConfig.PublicPrefix + "help" + "`"
				)
				.AddField(
					"ユーザーの情報を表示します",
					"`" + CommandConfig.PublicPrefix + "find" + " " + "[ユーザーID]" + "`"
				)
				.AddField(
					"画像をもとに絵文字を作成します",
					"`" + CommandConfig.PublicPrefix + "emoji" + " " + "[名前]" + " " + "(画像アップロード)" + "`"
				)
				.AddField(
					"招待URLを作成します",
					"`" + CommandConfig.PublicPrefix + "invite" + " " + "(チャンネル)" + "`"
				)
				.WithColor(new DiscordColor(0x00B06B))
				.WithTimestamp(DateTime.Now)
				.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
			await CommandObject.Member.SendMessageAsync(default, default, PublicEmbed);

			try {
				FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, CommandObject.Message.Author.Id);
				AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUsersID.RoleNum);
				Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				RoleLevel DBRoleLevel = (RoleLevel) Enum.Parse(typeof(RoleLevel), DBRolesNum.RoleLevel);

				if (DBRoleLevel == RoleLevel.Moderator) {
					DiscordEmbed ModeratorEmbed = new DiscordEmbedBuilder()
						.WithTitle("モデレーターコマンド")
						.WithDescription(string.Format("プレフィックスは {0} です", CommandConfig.ModeratorPrefix))
						.AddField(
							"Userデータベースに登録されているユーザー情報をリストにして表示します",
							"`" + CommandConfig.ModeratorPrefix + "db-userlist" + "`"
						)
						.AddField(
							"Roleデータベースに登録されているユーザー情報をリストにして表示します",
							"`" + CommandConfig.ModeratorPrefix + "db-rolelist" + "`"
						)
						.AddField(
							"Userデータベースにユーザーを追加します",
							"`" + CommandConfig.ModeratorPrefix + "db-useradd" + " " + "[名前]" + " " + "[ユーザーID]" + " " + "[役職登録番号]" + "`"
						)
						.AddField(
							"Roleデータベースに役職を追加します",
							"`" + CommandConfig.ModeratorPrefix + "db-roleadd" + " " + "[役職ID]" + " " + "[役職登録番号]" + " " + "[役職レベル(一般: 0, モデレーター: 1, Bot: 2)]" + "`"
						)
						.AddField(
							"Userデータベースに登録されているユーザーの役職を変更します",
							"`" + CommandConfig.ModeratorPrefix + "db-usercrole" + " " + "[ユーザーID]" + " " + "[役職登録番号]" + "`"
						)
						.AddField(
							"Userデータベースからユーザーを削除します",
							"`" + CommandConfig.ModeratorPrefix + "db-userdel" + " " + "[ユーザーID]" + "`"
						)
						.AddField(
							"Roleデータベースから役職を削除します",
							"`" + CommandConfig.ModeratorPrefix + "db-roledel" + " " + "[役職ID]" + "`"
						)
						.AddField(
							"LogChannelデータベースにログ送信用のチャンネル設定を追加します",
							"`" + CommandConfig.ModeratorPrefix + "db-logchadd" + " " + "[チャンネルID]" + "`"
						)
						.AddField(
							"LogChannelデータベースにログ送信用のチャンネル設定を変更します",
							"`" + CommandConfig.ModeratorPrefix + "db-clogch" + " " + "[チャンネルID]" + "`"
						)
						.WithColor(new DiscordColor(0xF6AA00))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
					await CommandObject.Member.SendMessageAsync(default, default, ModeratorEmbed);
				}
				if (CommandObject.Message.Author.Id == ClientConfig.BotownerId) {
					DiscordEmbed BotownerEmbed = new DiscordEmbedBuilder()
						.WithTitle("Bot管理者コマンド")
						.WithDescription(string.Format("プレフィックスは {0} です", CommandConfig.BotownerPrefix))
						.AddField(
							"Botを再接続します",
							"`" + CommandConfig.BotownerPrefix + "restart" + "`"
						)
						.AddField(
							"Botを終了します",
							"`" + CommandConfig.BotownerPrefix + "logout" + "`"
						)
						.WithColor(new DiscordColor(0x1971FF))
						.WithTimestamp(DateTime.Now)
						.WithFooter(string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username));
					await CommandObject.Member.SendMessageAsync(default, default, BotownerEmbed);
				}
			}
			catch (InvalidOperationException) {
				Log.Error("Invalid database element");
			}
		}
	}
}
