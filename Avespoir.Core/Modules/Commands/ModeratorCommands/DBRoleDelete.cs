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

				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
				FilterDefinition<Roles> DBGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, CommandObject.Guild.Id);

				try {
					FilterDefinition<Roles> DBRolesIDFilter = Builders<Roles>.Filter.Eq(Role => Role.uuid, msgs_ID);
					FilterDefinition<Roles> DBGuildRoleIDFilter = Builders<Roles>.Filter.And(DBGuildIDFilter, DBRolesIDFilter);
					Roles DBRolesID = await (await DBRolesCollection.FindAsync(DBGuildRoleIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBRolesID is null, processes will not be executed from here.

					if (!await Authentication.Confirmation(CommandObject)) {
						await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
						return;
					}

					await DBRolesCollection.DeleteOneAsync(DBRolesIDFilter).ConfigureAwait(false);

					DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRolesID.uuid);
					string ResultText = string.Format(CommandObject.Language.DBRoleDeleteSuccess, GuildRole.Name, DBRolesID.uuid);
					await CommandObject.Message.Channel.SendMessageAsync(ResultText);
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
