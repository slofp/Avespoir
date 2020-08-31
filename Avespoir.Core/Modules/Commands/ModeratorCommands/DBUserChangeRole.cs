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

		[Command("db-usercrole")]
		public async Task DBUserChangeRole(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				if (msgs.Length == 0) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.EmptyText);
					return;
				}

				ulong msgs_ID;
				uint msgs_RoleNum;

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

				IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
				FilterDefinition<AllowUsers> DBAllowUsersGuildIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.GuildID, CommandObject.Guild.Id);
				FilterDefinition<Roles> DBRolesGuildIDFilter = Builders<Roles>.Filter.Eq(Role => Role.GuildID, CommandObject.Guild.Id);

				try {
					FilterDefinition<AllowUsers> DBAllowUsersIDFilter = Builders<AllowUsers>.Filter.Eq(AllowUser => AllowUser.uuid, msgs_ID);
					FilterDefinition<AllowUsers> DBAllowUsersGuildIDIDFilter = Builders<AllowUsers>.Filter.And(DBAllowUsersGuildIDFilter, DBAllowUsersIDFilter);
					AllowUsers DBAllowUsersID = await (await DBAllowUsersCollection.FindAsync(DBAllowUsersGuildIDIDFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
					// if DBAllowUsersID is null, processes will not be executed from here.

					try {
						FilterDefinition<Roles> DBRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, msgs_RoleNum);
						FilterDefinition<Roles> DBRolesGuildIDNumFilter = Builders<Roles>.Filter.And(DBRolesGuildIDFilter, DBRolesNumFilter);
						Roles DBRolesNum = await (await DBRolesCollection.FindAsync(DBRolesGuildIDNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
						// if DBRolesNum is null, processes will not be executed from here.

						try {
							FilterDefinition<Roles> DBBeforeRolesNumFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleNum, DBAllowUsersID.RoleNum);
							FilterDefinition<Roles> DBBeforeRolesGuildIDNumFilter = Builders<Roles>.Filter.And(DBRolesGuildIDFilter, DBBeforeRolesNumFilter);
							Roles DBBeforeRolesNum = await (await DBRolesCollection.FindAsync(DBBeforeRolesGuildIDNumFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);
							// if DBBeforeRolesNum is null, processes will not be executed from here.

							if (!await Authentication.Confirmation(CommandObject)) {
								await CommandObject.Channel.SendMessageAsync(CommandObject.Language.AuthFailure);
								return;
							}

							UpdateDefinition<AllowUsers> UpdateAllowUserRole = Builders<AllowUsers>.Update.Set(AllowUser => AllowUser.RoleNum, msgs_RoleNum);
							await DBAllowUsersCollection.UpdateOneAsync(DBAllowUsersGuildIDIDFilter, UpdateAllowUserRole).ConfigureAwait(false);

							DiscordMember GuildMember = await CommandObject.Guild.GetMemberAsync(msgs_ID);

							DiscordRole GuildAfterRole = CommandObject.Guild.GetRole(DBRolesNum.uuid);
							await GuildMember.GrantRoleAsync(GuildAfterRole);

							DiscordRole GuildBeforeRole = CommandObject.Guild.GetRole(DBBeforeRolesNum.uuid);
							await GuildMember.RevokeRoleAsync(GuildBeforeRole);

							string ResultText = string.Format(CommandObject.Language.DBUserChangeRoleSuccess, GuildMember.Username + "#" + GuildMember.Discriminator, GuildBeforeRole.Name, GuildAfterRole.Name);
							await CommandObject.Message.Channel.SendMessageAsync(ResultText);

							RoleLevel DBRoleLevel = Enum.Parse<RoleLevel>(DBRolesNum.RoleLevel);
							bool GuildLeaveBan = await DatabaseMethods.LeaveBanFind(CommandObject.Guild.Id).ConfigureAwait(false);
							if (GuildLeaveBan) {
								if (DBRoleLevel == RoleLevel.Public) {
									DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
										.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
										.AddField(
											CommandObject.Language.DMEmbed_Public1,
											CommandObject.Language.DMEmbed_Public2
										)
										.AddField(
											CommandObject.Language.DMEmbed_Public3,
											CommandObject.Language.DMEmbed_Public4
										)
										.AddField(
											CommandObject.Language.DMEmbed_LeaveBan1,
											string.Format(CommandObject.Language.DMEmbed_LeaveBan2, GuildAfterRole.Name)
										)
										.WithColor(new DiscordColor(0x00B06B))
										.WithFooter(
											string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
										);
									await GuildMember.SendMessageAsync(default, default, WelcomeEmbed);

									return;
								}
								else if (DBRoleLevel == RoleLevel.Moderator) {
									DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
										.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
										.AddField(
											CommandObject.Language.DMEmbed_Moderator1,
											CommandObject.Language.DMEmbed_Moderator2
										)
										.AddField(
											CommandObject.Language.DMEmbed_Moderator3,
											string.Format(CommandObject.Language.DMEmbed_Moderator4, CommandObject.Client.CurrentUser.Username)
										)
										.AddField(
											CommandObject.Language.DMEmbed_Moderator5,
											CommandObject.Language.DMEmbed_Moderator6
										)
										.AddField(
											CommandObject.Language.DMEmbed_LeaveBan1,
											string.Format(CommandObject.Language.DMEmbed_LeaveBan2, GuildAfterRole.Name)
										)
										.WithColor(new DiscordColor(0x00B06B))
										.WithFooter(
											string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
										);
									await GuildMember.SendMessageAsync(default, default, WelcomeEmbed);

									return;
								}
							}
							else {
								if (DBRoleLevel == RoleLevel.Public) {
									DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
										.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
										.AddField(
											CommandObject.Language.DMEmbed_Public1,
											CommandObject.Language.DMEmbed_Public2
										)
										.AddField(
											CommandObject.Language.DMEmbed_Public3,
											CommandObject.Language.DMEmbed_Public4
										)
										.WithColor(new DiscordColor(0x00B06B))
										.WithFooter(
											string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
										);
									await GuildMember.SendMessageAsync(default, default, WelcomeEmbed);

									return;
								}
								else if (DBRoleLevel == RoleLevel.Moderator) {
									DiscordEmbed WelcomeEmbed = new Discord​Embed​Builder()
										.WithTitle(string.Format(CommandObject.Language.DBUserChangeRoleEmbedTitle, GuildAfterRole.Name))
										.AddField(
											CommandObject.Language.DMEmbed_Moderator1,
											CommandObject.Language.DMEmbed_Moderator2
										)
										.AddField(
											CommandObject.Language.DMEmbed_Moderator3,
											string.Format(CommandObject.Language.DMEmbed_Moderator4, CommandObject.Client.CurrentUser.Username)
										)
										.AddField(
											CommandObject.Language.DMEmbed_Moderator5,
											CommandObject.Language.DMEmbed_Moderator6
										)
										.WithColor(new DiscordColor(0x00B06B))
										.WithFooter(
											string.Format("{0} Bot", CommandObject.Client.CurrentUser.Username)
										);
									await GuildMember.SendMessageAsync(default, default, WelcomeEmbed);

									return;
								}
							}
						}
						catch (InvalidOperationException) {
							await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.BeforeRoleNotFound);
							return;
						}
					}
					catch (InvalidOperationException) {
						await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.RoleNumberNotFound);
						return;
					}
				}
				catch (InvalidOperationException) {
					await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.IdNotRegisted);
					return;
				}
			}
			catch (IndexOutOfRangeException) {
				await CommandObject.Message.Channel.SendMessageAsync(CommandObject.Language.TypingMissed);
				return;
			}
		}
	}
}
