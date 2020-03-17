using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Database;
using AvespoirTest.Core.Database.Enums;
using AvespoirTest.Core.Database.Schemas;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("help")]
		public async Task Help(CommandObjects CommandObject) {
			IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
			IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

			try {
				FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, CommandObject.Message.Author.Id);
				AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUsersID.RoleNum);
				Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				RoleLevel DBRoleLevel = (RoleLevel) Enum.Parse(typeof(RoleLevel), DBRolesNum.RoleLevel);

				await CommandObject.Channel.SendMessageAsync($"{CommandObject.Member.Mention} DMをご確認ください！");

				if (DBRoleLevel == RoleLevel.Public || DBRoleLevel == RoleLevel.Moderator) {
					DiscordEmbed PublicEmbed = new DiscordEmbedBuilder()
						.WithTitle("一般コマンド")
						.WithDescription(string.Format("プレフィックスは {0} です", CommandConfig.PublicPrefix))
						.AddField(
							CommandConfig.PublicPrefix + "ping",
							"Pingを測ります"
						)
						.AddField(
							CommandConfig.PublicPrefix + "ver",
							"Botのバージョンを表示します"
						)
						.AddField(
							CommandConfig.PublicPrefix + "help",
							"コマンド一覧を表示します"
						)
						.AddField(
							CommandConfig.PublicPrefix + "find"+ " " + "[ユーザーID]",
							"ユーザーの情報を表示します"
						)
						.AddField(
							CommandConfig.PublicPrefix + "emoji"+ " " + "[名前]" + " " + "(画像アップロード)",
							"画像をもとに絵文字を作成します"
						)
						.WithColor(new DiscordColor(0x00B06B));
					await CommandObject.Member.SendMessageAsync(default, default, PublicEmbed);
				}
				if (DBRoleLevel == RoleLevel.Moderator) {
					DiscordEmbed ModeratorEmbed = new DiscordEmbedBuilder()
						.WithTitle("モデレーターコマンド")
						.WithDescription(string.Format("プレフィックスは {0} です", CommandConfig.ModeratorPrefix))
						.AddField(
							CommandConfig.ModeratorPrefix + "db-userlist",
							"Userデータベースに登録されているユーザー情報をリストにして表示します"
						)
						.AddField(
							CommandConfig.ModeratorPrefix + "db-rolelist",
							"Roleデータベースに登録されているユーザー情報をリストにして表示します"
						)
						.AddField(
							CommandConfig.ModeratorPrefix + "db-useradd" + " " + "[名前]" + " " + "[ユーザーID]" + " " + "[役職登録番号]",
							"Userデータベースにユーザーを追加します"
						)
						.AddField(
							CommandConfig.ModeratorPrefix + "db-roleadd" + " " + "[役職ID]" + " " + "[役職登録番号]" + " " + "[役職レベル(一般: 0, モデレーター: 1, Bot: 2)]",
							"Roleデータベースに役職を追加します"
						)
						.AddField(
							CommandConfig.ModeratorPrefix + "db-userdel" + " " + "[ユーザーID]",
							"Userデータベースからユーザーを削除します"
						)
						.AddField(
							CommandConfig.ModeratorPrefix + "db-roledel" + " " + "[役職ID]",
							"Roleデータベースから役職を削除します"
						)
						.WithColor(new DiscordColor(0xF6AA00));
					await CommandObject.Member.SendMessageAsync(default, default, ModeratorEmbed);
				}
				if (CommandObject.Message.Author.Id == ClientConfig.BotownerId) {
					DiscordEmbed BotownerEmbed = new DiscordEmbedBuilder()
						.WithTitle("Bot管理者コマンド")
						.WithDescription(string.Format("プレフィックスは {0} です", CommandConfig.BotownerPrefix))
						.AddField(
							CommandConfig.BotownerPrefix + "db-logchanneladd" + " " + "[サーバーID]" + " " + "[チャンネルID]",
							"LogChannelデータベースにログ送信用のチャンネル設定を追加します"
						)
						.AddField(
							CommandConfig.BotownerPrefix + "restart",
							"Botを再接続します"
						)
						.AddField(
							CommandConfig.BotownerPrefix + "logout",
							"Botを終了します"
						)
						.WithColor(new DiscordColor(0x1971FF));
					await CommandObject.Member.SendMessageAsync(default, default, BotownerEmbed);
				}
			}
			catch (InvalidOperationException) {
				await CommandObject.Channel.SendMessageAsync("エラーが発生しました");
				new ErrorLog("Invalid database element");
			}
		}
	}
}
