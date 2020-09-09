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
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				ulong msgs_ID;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
				FilterDefinition<AllowUsers> DBAllowUsersGuildIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.GuildID, CommandObject.Guild.Id);

				try {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, msgs_ID);
					FilterDefinition<AllowUsers> DBAllowUsersGuildIDIDFilter = Builders<AllowUsers>.Filter.And(DBAllowUsersGuildIDFilter, DBAllowUsersIDFilter);
					AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBAllowUsersID is null, processes will not be executed from here.

					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
						return;
					}

					await DBAllowUsersCollection.DeleteOneAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false);
					try {
						DiscordMember DeleteGuildMember = await CommandObject.Guild.GetMemberAsync(DBAllowUsersID.uuid);
						string KickReason = string.Format(CommandObject.Language.KickReason, CommandObject.Member.Username + "#" + CommandObject.Member.Discriminator);
						await DeleteGuildMember.RemoveAsync(KickReason);
					}
					finally {
						string ResultText = string.Format(CommandObject.Language.DBUserDeleteSuccess, DBAllowUsersID.Name, DBAllowUsersID.uuid);
						await CommandObject.Message.Channel.SendMessageAsync(ResultText);
					}
				}
				catch (InvalidOperationException) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdNotRegisted);
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
			}
		}
	}
}
