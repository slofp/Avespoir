using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Database;
using AvespoirTest.Core.Database.Enums;
using AvespoirTest.Core.Database.Schemas;
using AvespoirTest.Core.Modules.Utils;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-roleadd")]
		public async Task DBRoleAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				ulong msgs_ID;
				uint msgs_RoleNum;
				int msgs_RoleLevel;
				RoleLevel intRoleLevel;

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

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleLevelが空白またはNullです");
					return;
				}
				if (!int.TryParse(msgs[2], out msgs_RoleLevel)) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleLevelは数字でなければいけません");
					return;
				}
				
				intRoleLevel = (RoleLevel) Enum.ToObject(typeof(RoleLevel), msgs_RoleLevel);
				if (string.IsNullOrWhiteSpace(Enum.GetName(typeof(RoleLevel), intRoleLevel))) {
					await CommandObject.Message.Channel.SendMessageAsync("RoleLevelが適切ではありません");
					return;
				}

				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

				try {
					FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, msgs_RoleNum);
					Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBRolesNum is null, InvalidOperationException is a normal operation.

					await CommandObject.Message.Channel.SendMessageAsync("そのRoleNumberはすでに登録されています");
					return;
				}
				catch (InvalidOperationException) {
					try {
						FilterDefinition<Roles> DBRolesIDFilter = Builders<Roles>.Filter.Eq(Role => Role.uuid, msgs_ID);
						Roles DBRolesID = await (await DBRolesCollection.FindAsync(DBRolesIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRolesID is null, InvalidOperationException is a normal operation.

						await CommandObject.Message.Channel.SendMessageAsync("そのIDはすでに登録されています");
						return;
					}
					catch (InvalidOperationException) {
						Roles InsertRoleData = new Roles { 
							uuid = msgs_ID,
							RoleNum = msgs_RoleNum,
							RoleLevel = Enum.GetName(typeof(RoleLevel), intRoleLevel)
						};

						await DBRolesCollection.InsertOneAsync(InsertRoleData).ConfigureAwait(false);

						DiscordRole GuildRole = CommandObject.Guild.GetRole(InsertRoleData.uuid);
						string ResultText = string.Format("Roleデータベースに\nID: {0}({1})\nRoleNumber: {2}\nRoleLevel: {3}\nで登録しました！", InsertRoleData.uuid, GuildRole.Name, InsertRoleData.RoleNum, InsertRoleData.RoleLevel);
						await CommandObject.Message.Channel.SendMessageAsync(ResultText);
					}
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync("必要箇所が入力されていません");
			}
		}
	}
}
