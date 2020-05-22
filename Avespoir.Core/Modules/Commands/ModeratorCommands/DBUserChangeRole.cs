using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-usercrole")]
		public async Task DBUserChangeRole(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
					return;
				}

				ulong msgs_ID;
				uint msgs_RoleNum;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
					return;
				}
				if (!ulong.TryParse(msgs[0], out msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleNumberが空白またはNullです");
					return;
				}
				if (!uint.TryParse(msgs[1], out msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleNumberは数字でなければいけません");
					return;
				}

				IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

				try {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, msgs_ID);
					AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBAllowUsersID is null, processes will not be executed from here.

					try {
						FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, msgs_RoleNum);
						Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRolesNum is null, processes will not be executed from here.

						try {
							FilterDefinition<Roles> DBBeforeRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUsersID.RoleNum);
							Roles DBBeforeRolesNum = await (await DBRolesCollection.FindAsync(DBBeforeRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
							// if DBBeforeRolesNum is null, processes will not be executed from here.

							if (!await Authentication.Confirmation(CommandObject)) {
								await CommandObject.Channel.SendMessageAsync("認証に失敗しました、初めからやり直してください");
								return;
							}

							UpdateDefinition<AllowUsers> UpdateAllowUserRole = Builders<AllowUsers>.Update.Set(AllowUser => AllowUser.RoleNum, msgs_RoleNum);
							await DBAllowUsersCollection.UpdateOneAsync(DBAllowUsersIDFilter, UpdateAllowUserRole).ConfigureAwait(false);

							DiscordMember GuildMember = await CommandObject.Guild.GetMemberAsync(msgs_ID);

							DiscordRole GuildAfterRole = CommandObject.Guild.GetRole(DBRolesNum.uuid);
							await GuildMember.GrantRoleAsync(GuildAfterRole);

							DiscordRole GuildBeforeRole = CommandObject.Guild.GetRole(DBBeforeRolesNum.uuid);
							await GuildMember.RevokeRoleAsync(GuildBeforeRole);

							string ResultText = string.Format("{0}のRoleを{1}から{2}に変更しました！", GuildMember.Username + "#" + GuildMember.Discriminator, GuildBeforeRole.Name, GuildAfterRole.Name);
							await CommandObject.Message.Channel.SendMessageAsync(ResultText);

							RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRolesNum.RoleLevel);
							if (DBRoleLevel == RoleLevel.Public) {
								DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
									.WithTitle(string.Format("Roleが{0}に変更になりました！ {0}では次のことが許可されています！", GuildAfterRole.Name))
									.AddField(
										"サーバーの操作ができます",
										"ほぼ自由にサーバーを操作できます"
									)
									.AddField(
										"すべてのチャンネルを読んだり送信することができます",
										"これはあなたが認定された証です"
									)
									.AddField(
										"抜けたらBANされます",
										string.Format("現在{0}は抜けたらBANされます", GuildAfterRole.Name)
									)
									.WithColor(new DiscordColor(0x00B06B))
									.WithFooter(
										string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
									);
								await GuildMember.SendMessageAsync(default, default, WelcomeEmbed);

								return;
							}
							else if (DBRoleLevel == RoleLevel.Moderator) {
								DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
									.WithTitle(string.Format("Roleが{0}に変更になりました！ {0}では次のことが許可されています！", GuildAfterRole.Name))
									.AddField(
										"サーバーの操作ができます",
										"今日からあなたはここの管理者です"
									)
									.AddField(
										"データベースへの追加、変更、削除等が行えます",
										string.Format("{0}Botのすべてのコマンドを使うことができます", CommandObject.Client.CurrentUser.Username)
									)
									.AddField(
										"すべてのチャンネルを読んだり送信することができます",
										"これはあなたが認定された証です"
									)
									.AddField(
										"抜けたらBANされます",
										string.Format("現在{0}は抜けたらBANされます", GuildAfterRole.Name)
									)
									.WithColor(new DiscordColor(0x00B06B))
									.WithFooter(
										string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
									);
								await GuildMember.SendMessageAsync(default, default, WelcomeEmbed);

								return;
							}
						}
						catch (InvalidOperationException) {
							await CommandObject.Message.Channel.SendMessageAsync("Roleデータベースに元のRoleが存在しません");
							return;
						}
					}
					catch (InvalidOperationException) {
						await CommandObject.Message.Channel.SendMessageAsync("RoleNumberがRoleデータベースに存在しません");
						return;
					}
				}
				catch (InvalidOperationException) {
					await CommandObject.Message.Channel.SendMessageAsync("そのIDは登録されていません");
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync("必要箇所が入力されていません");
				return;
			}
		}
	}
}
