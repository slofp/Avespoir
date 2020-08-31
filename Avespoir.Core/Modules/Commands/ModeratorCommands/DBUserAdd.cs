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

		[Command("db-useradd")]
		public async Task DBUserAdd(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				string msgs_Name;
				ulong msgs_ID;
				uint msgs_RoleNum;

				if (string.IsNullOrWhiteSpace(msgs[0])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyName);
					return;
				}
				msgs_Name = msgs[0];

				if (string.IsNullOrWhiteSpace(msgs[1])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyId);
					return;
				}
				if (!ulong.TryParse(msgs[1], out msgs_ID)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdCouldntParse);
					return;
				}

				if (string.IsNullOrWhiteSpace(msgs[2])) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyRoleNumber);
					return;
				}
				if (!uint.TryParse(msgs[2], out msgs_RoleNum)) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotNumber);
					return;
				}

				IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
				FilterDefinition<AllowUsers> DBAllowUsersGuildIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.GuildID, CommandObject.Guild.Id);
				FilterDefinition<Roles> DBRolesGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, CommandObject.Guild.Id);

				try {
					FilterDefinition<AllowUsers> DBAllowUsersNameFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.Name, msgs_Name);
					FilterDefinition<AllowUsers> DBAllowUsersGuildIDNameFilter = Builders<AllowUsers>.Filter.And(DBAllowUsersGuildIDFilter, DBAllowUsersNameFilter);

					AllowUsers DBAllowUsersName = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDNameFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBAllowUsersName is null, InvalidOperationException is a normal operation.

					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.NameRegisted);
					return;
				}
				catch (InvalidOperationException) {
					try {
						FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, msgs_ID);
						FilterDefinition<AllowUsers> DBAllowUsersGuildIDIDFilter = Builders<AllowUsers>.Filter.And(DBAllowUsersGuildIDFilter, DBAllowUsersIDFilter);
						AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRolesNum is null, InvalidOperationException is a normal operation.

						await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdRegisted);
						return;
					}
					catch (InvalidOperationException) {
						try {
							FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, msgs_RoleNum);
							FilterDefinition<Roles> DBRolesGuildIDNumFilter = Builders<Roles>.Filter.And(DBRolesGuildIDFilter, DBRolesNumFilter);
							Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesGuildIDNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
							// if DBRolesNum is null, processes will not be executed from here.

							if (!await Authentication.Confirmation(CommandObject)) {
								await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
								return;
							}

							AllowUsers InsertAllowUserData = new AllowUsers {
								GuildID = CommandObject.Guild.Id,
								Name = msgs_Name,
								uuid = msgs_ID,
								RoleNum = msgs_RoleNum
							};
							await DBAllowUsersCollection.InsertOneAsync(InsertAllowUserData).ConfigureAwait(false);

							DiscordRole GuildRole = CommandObject.Guild.GetRole(DBRolesNum.uuid);
							string ResultText = string.Format(CommandObject.Language.DBUserAddSuccess, InsertAllowUserData.Name, InsertAllowUserData.uuid, InsertAllowUserData.RoleNum, GuildRole.Name);
							await CommandObject.Message.Channel.SendMessageAsync(ResultText);
						}
						catch (InvalidOperationException) {
							await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotFound);
							return;
						}
					}
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
			}
		}
	}
}
