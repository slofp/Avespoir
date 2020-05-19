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

		[Command("db-clogch")]
		public async Task DBLogChannelChange(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
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

					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync("認証に失敗しました、初めからやり直してください");
						return;
					}

					UpdateDefinition<LogChannels> UpdateLogChannel = Builders<LogChannels>.Update.Set(LogChannel => LogChannel.ChannelID, msgs_ChannelID);
					await DBLogChannelsCollection.UpdateOneAsync(DBLogChannelsGuildIDFilter, UpdateLogChannel).ConfigureAwait(false);

					string ResultText = string.Format("LogChannelを{0}に変更しました！", msgs_ChannelID);
					await CommandObject.Message.Channel.SendMessageAsync(ResultText);
				}
				catch (InvalidOperationException) {
					await CommandObject.Message.Channel.SendMessageAsync("サーバーに登録されていません");
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync("必要箇所が入力されていません");
			}
		}
	}
}
