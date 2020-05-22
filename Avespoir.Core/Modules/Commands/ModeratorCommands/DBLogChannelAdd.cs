using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class ModeratorCommands {

		[Command("db-logchadd")]
		public async Task DBLogChannelAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync("何も入力されていません");
					return;
				}

				ulong msgs_ChannelID;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
					return;
				}
				if (!ulong.TryParse(msgs[0], out msgs_ChannelID)) {
					await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
					return;
				}

				ulong msgs_GuildID = CommandObject.Guild.Id;

				IMongoCollection<LogChannels> DBLogChannelsCollection = MongoDBClient.Database.GetCollection<LogChannels>(typeof(LogChannels).Name);

				try {
					FilterDefinition<LogChannels> DBLogChannelsGuildIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, msgs_GuildID);
					LogChannels DBLogChannelsGuildID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsGuildIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBLogChannelsGuildID is null, InvalidOperationException is a normal operation.

					await CommandObject.Message.Channel.SendMessageAsync("すでに登録されています");
					return;
				}
				catch (InvalidOperationException) {
					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync("認証に失敗しました、初めからやり直してください");
						return;
					}

					LogChannels InsertLogChannelData = new LogChannels {
						GuildID = msgs_GuildID,
						ChannelID = msgs_ChannelID
					};

					await DBLogChannelsCollection.InsertOneAsync(InsertLogChannelData).ConfigureAwait(false);

					string ResultText = string.Format("LogChannelデータベースに\nChannelID: {0}\nで登録しました！", InsertLogChannelData.ChannelID);
					await CommandObject.Message.Channel.SendMessageAsync(ResultText);
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync("必要箇所が入力されていません");
			}
		}
	}
}
