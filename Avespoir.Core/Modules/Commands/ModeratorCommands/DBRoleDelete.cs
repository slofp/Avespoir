using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using DSharpPlus.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-roledel")]
		public async Task DBRoleDelete(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
					return;
				}

				ulong msgs_ID;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
					return;
				}
				if (!ulong.TryParse(msgs[0], out msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
					return;
				}

				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

				try {
					FilterDefinition<Roles> DBRolesIDFilter = Builders<Roles>.Filter.Eq(Role => Role.uuid, msgs_ID);
					Roles DBRolesID = await (await DBRolesCollection.FindAsync(DBRolesIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBRolesID is null, processes will not be executed from here.

					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync("認証に失敗しました、初めからやり直してください");
						return;
					}

					await DBRolesCollection.DeleteOneAsync(DBRolesIDFilter).ConfigureAwait(false);

					DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRolesID.uuid);
					string ResultText = string.Format("名前: {0}\nID: {1}\nをRoleデータベースから削除しました", GuildRole.Name, DBRolesID.uuid);
					await CommandObject.Message.Channel.SendMessageAsync(ResultText);
				}
				catch (InvalidOperationException) {
					await CommandObject.Message.Channel.SendMessageAsync("そのIDは登録されていません");
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync("必要箇所が入力されていません");
			}
		}
	}
}
