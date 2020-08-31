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

		[Command("db-roleadd")]
		public async Task DBRoleAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				ulong msgs_ID;
				uint msgs_RoleNum;
				int msgs_RoleLevel;
				RoleLevel intRoleLevel;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[0], out msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[1], out msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotNumber);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyRoleLevel);
					return;
				}
				if (!int.TryParse(msgs[2], out msgs_RoleLevel)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleLevelNotNumber);
					return;
				}
				
				intRoleLevel = (RoleLevel) Enum.ToObject(typeof(RoleLevel), msgs_RoleLevel);
				if (string.IsNullOrWhiteSpace(Enum.GetName(typeof(RoleLevel), intRoleLevel))) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleLevelNotFound);
					return;
				}

				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
				FilterDefinition<Roles> DBGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, CommandObject.Guild.Id);

				try {
					FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, msgs_RoleNum);
					FilterDefinition<Roles> DBGuildRoleNumFilter = Builders<Roles>.Filter.And(DBGuildIDFilter, DBRolesNumFilter);
					Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBGuildRoleNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBRolesNum is null, InvalidOperationException is a normal operation.

					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberRegisted);
					return;
				}
				catch (InvalidOperationException) {
					try {
						FilterDefinition<Roles> DBRolesIDFilter = Builders<Roles>.Filter.Eq(Role => Role.uuid, msgs_ID);
						FilterDefinition<Roles> DBGuildRoleIDFilter = Builders<Roles>.Filter.And(DBGuildIDFilter, DBRolesIDFilter);
						Roles DBRolesID = await (await DBRolesCollection.FindAsync(DBGuildRoleIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRolesID is null, InvalidOperationException is a normal operation.

						await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdRegisted);
						return;
					}
					catch (InvalidOperationException) {
						if (!await Authentication.Confirmation(CommandObject)) {
							await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
							return;
						}

						Roles InsertRoleData = new Roles { 
							GuildID = CommandObject.Guild.Id,
							uuid = msgs_ID,
							RoleNum = msgs_RoleNum,
							RoleLevel = Enum.GetName(typeof(RoleLevel), intRoleLevel)
						};

						await DBRolesCollection.InsertOneAsync(InsertRoleData).ConfigureAwait(false);

						DiscordRole GuildRole = CommandObject.Guild.GetRole(InsertRoleData.uuid);
						string ResultText = string.Format(CommandObject.Language.DBRoleAddSuccess, InsertRoleData.uuid, GuildRole.Name, InsertRoleData.RoleNum, InsertRoleData.RoleLevel);
						await CommandObject.Message.Channel.SendMessageAsync(ResultText);
					}
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
			}
		}
	}
}
