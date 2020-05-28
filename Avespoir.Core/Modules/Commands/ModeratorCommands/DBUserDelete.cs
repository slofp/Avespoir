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

		[Command("db-userdel")]
		public async Task DBUserDelete(CommandObjects CommandObject) {
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

				IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);

				try {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, msgs_ID);
					AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBAllowUsersID is null, processes will not be executed from here.

					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync("認証に失敗しました、初めからやり直してください");
						return;
					}

					await DBAllowUsersCollection.DeleteOneAsync(DBAllowUsersIDFilter).ConfigureAwait(false);
					DiscordMember DeleteGuildMember = await CommandObject.Guild.GetMemberAsync(DBAllowUsersID.uuid);
					string KickReason = string.Format("{0}によってUserデータベースから削除されたため", CommandObject.Member.Username + "#" + CommandObject.Member.Discriminator);
					await DeleteGuildMember.RemoveAsync(KickReason);

					string ResultText = string.Format("名前: {0}\nID: {1}\nをUserデータベースから削除しました", DBAllowUsersID.Name, DBAllowUsersID.uuid);
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
