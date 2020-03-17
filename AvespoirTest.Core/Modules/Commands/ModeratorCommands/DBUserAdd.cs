using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Database;
using AvespoirTest.Core.Database.Schemas;
using AvespoirTest.Core.Modules.Utils;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-useradd")]
		public async Task DBUserAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				string msgs_Name;
				ulong msgs_ID;
				uint msgs_RoleNum;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync("名前が空白またはNullです");
					return;
				}
				msgs_Name = msgs[0];

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
					return;
				}
				if (!ulong.TryParse(msgs[1], out msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleNumberが空白またはNullです");
					return;
				}
				if (!uint.TryParse(msgs[2], out msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleNumberは数字でなければいけません");
					return;
				}

				IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

				try {
					FilterDefinition<AllowUsers> DBAllowUsersNameFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.Name, msgs_Name);
					AllowUsers DBAllowUsersName = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersNameFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBAllowUsersName is null, InvalidOperationException is a normal operation.

					await CommandObject.Message.Channel.SendMessageAsync("その名前はすでに登録されています");
					return;
				}
				catch (InvalidOperationException) {
					try {
						FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, msgs_ID);
						AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRolesNum is null, InvalidOperationException is a normal operation.

						await CommandObject.Message.Channel.SendMessageAsync("そのIDはすでに登録されています");
						return;
					}
					catch (InvalidOperationException) {
						try {
							FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, msgs_RoleNum);
							Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
							// if DBRolesNum is null, processes will not be executed from here.

							AllowUsers InsertAllowUserData = new AllowUsers {
								Name = msgs_Name,
								uuid = msgs_ID,
								RoleNum = msgs_RoleNum
							};
							await DBAllowUsersCollection.InsertOneAsync(InsertAllowUserData).ConfigureAwait(false);

							DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRolesNum.uuid);
							string ResultText = string.Format("Userデータベースに\n名前: {0}\nID: {1}\nRoleNumber: {2}({3})\nで登録しました！", InsertAllowUserData.Name, InsertAllowUserData.uuid, InsertAllowUserData.RoleNum, GuildRole.Name);
							await CommandObject.Message.Channel.SendMessageAsync(ResultText);
						}
						catch (InvalidOperationException) {
							await CommandObject.Message.Channel.SendMessageAsync("RoleNumberがRoleデータベースに存在しません");
							return;
						}
					}
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync("必要箇所が入力されていません");
			}
		}
	}
}
