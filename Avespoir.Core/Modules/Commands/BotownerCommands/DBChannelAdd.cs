using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("db-logchanneladd")]
		public async Task DBChannelAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				ulong msgs_GuildID, msgs_ChannelID;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
					return;
				}
				if (!ulong.TryParse(msgs[0], out msgs_GuildID)) {
					await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync("IDが空白またはNullです");
					return;
				}
				if (!ulong.TryParse(msgs[1], out msgs_ChannelID)) {
					await CommandObject.Message.Channel.SendMessageAsync("IDは数字でなければいけません");
					return;
				}

				IMongoCollection<LogChannels> DBLogChannelsCollection = MongoDBClient.Database.GetCollection<LogChannels>(typeof(LogChannels).Name);

				try {
					FilterDefinition<LogChannels> DBLogChannelsGuildIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, msgs_GuildID);
					LogChannels DBLogChannelsGuildID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsGuildIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBLogChannelsGuildID is null, InvalidOperationException is a normal operation.

					await CommandObject.Message.Channel.SendMessageAsync("そのGuildIDはすでに登録されています");
					return;
				}
				catch (InvalidOperationException) {
					try {
						FilterDefinition<LogChannels> DBLogChannelsChannelIDFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.ChannelID, msgs_ChannelID);
						LogChannels DBLogChannelsChannelID = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsChannelIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBLogChannelsChannelID is null, InvalidOperationException is a normal operation.

						await CommandObject.Message.Channel.SendMessageAsync("そのChannelIDはすでに登録されています");
						return;
					}
					catch (InvalidOperationException) {
						LogChannels InsertLogChannelData = new LogChannels {
							GuildID = msgs_GuildID,
							ChannelID = msgs_ChannelID
						};

						await DBLogChannelsCollection.InsertOneAsync(InsertLogChannelData).ConfigureAwait(false);

						string ResultText = string.Format("LogChannelデータベースに\nGuildID: {0}\nChannelID: {1}\nで登録しました！", InsertLogChannelData.GuildID, InsertLogChannelData.ChannelID);
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
