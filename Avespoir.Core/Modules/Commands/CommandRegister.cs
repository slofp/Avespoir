using Avespoir.Core.Attributes;
using Avespoir.Core.JsonScheme;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avespoir.Core.Configs;

namespace Avespoir.Core.Modules.Commands {

	delegate Task CommandDelegate(CommandObjects CommandObject);

	class CommandRegister {

		static async Task ExcuteCommands<CommandsClass>(CommandObjects CommandObject, string CommandText) where CommandsClass : class {
			CommandDelegate Command_Delegate = null;
			Type CommandsType = typeof(CommandsClass);

			MethodInfo[] CommandsMethodInfo = CommandsType.GetMethods();
			foreach(MethodInfo CommandMethodInfo in CommandsMethodInfo) {
				Attribute GetCommandAttribute = Attribute.GetCustomAttribute(CommandMethodInfo, typeof(CommandAttribute));

				CommandAttribute Command_Attribute = GetCommandAttribute as CommandAttribute;
				if (Command_Attribute == null) {
					continue;
				}
				if (Command_Attribute.CommandName != CommandText) {
					continue;
				}
				if (CommandMethodInfo.ReturnType != typeof(Task)) {
					Log.Error("Return value is not regular expression.");
					continue;
				}

				Command_Delegate = Delegate.CreateDelegate(typeof(CommandDelegate), null, CommandMethodInfo) as CommandDelegate;
				break;
			}

			if (Command_Delegate != null) {
				await Command_Delegate.Invoke(CommandObject);
			}
			else {
				Log.Error("Commnad Not Found.");
			}
		}

		#region Command checker

		internal static async Task PublicCommands(MessageCreateEventArgs Message_Objects) {
			Log.Debug("PublicCommand check");
			if (Message_Objects.Channel.IsPrivate) return;
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string GuildPublicPrefix = await DatabaseMethods.PublicPrefixFind(CommandObject.Guild.Id).ConfigureAwait(false);
			if (GuildPublicPrefix == null) GuildPublicPrefix = CommandConfig.PublicPrefix;

			string CommandPrefix = CommandObject.CommandArgs[0].Substring(0, GuildPublicPrefix.Length);
			string CommandText = CommandObject.CommandArgs[0].Substring(GuildPublicPrefix.Length);

			if (CommandPrefix != GuildPublicPrefix) return;
			if (Message_Objects.Author.IsBot) return;

			await ExcuteCommands<PublicCommands>(CommandObject, CommandText).ConfigureAwait(false);
		}

		internal static async Task ModeratorCommands(MessageCreateEventArgs Message_Objects) {
			Log.Debug("ModeratorCommand check");
			if (Message_Objects.Channel.IsPrivate) return;
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string GuildModeratorPrefix = await DatabaseMethods.ModeratorPrefixFind(CommandObject.Guild.Id).ConfigureAwait(false);
			if (GuildModeratorPrefix == null) GuildModeratorPrefix = CommandConfig.ModeratorPrefix;

			string CommandPrefix = CommandObject.CommandArgs[0].Substring(0, GuildModeratorPrefix.Length);
			string CommandText = CommandObject.CommandArgs[0].Substring(GuildModeratorPrefix.Length);

			if (CommandPrefix != GuildModeratorPrefix) return;
			if (Message_Objects.Author.IsBot) return;

			if (Message_Objects.Message.Author.Id == Message_Objects.Guild.Owner.Id) {
				await ExcuteCommands<ModeratorCommands>(CommandObject, CommandText).ConfigureAwait(false);
			}
			else {
				bool ModCheck = false;

				IMongoCollection<Roles> DBRolesCollection = MongoDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);
				FilterDefinition<Roles> DBRoleFilter = Builders<Roles>.Filter.Eq(Role => Role.RoleLevel, Enum.GetName(typeof(RoleLevel), RoleLevel.Moderator));

				List<Roles> DBRoleList = await (await DBRolesCollection.FindAsync(DBRoleFilter).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);
				foreach (Roles DBRole in DBRoleList) {
					DiscordMember GuildMember = await Message_Objects.Guild.GetMemberAsync(Message_Objects.Message.Author.Id);

					List<DiscordRole> GuildRoleList = GuildMember.Roles.ToList();
					foreach (DiscordRole GuildRole in GuildRoleList) {
						if (GuildRole.Id == DBRole.uuid) {
							ModCheck = true;
							break;
						}
						else continue;
					}

					if (ModCheck) break;
					else continue;
				}

				if (!ModCheck) {
					Log.Info("The member who has not been granted moderator roles registered in the database tried to access the moderator command.");
					return;
				}

				await ExcuteCommands<ModeratorCommands>(CommandObject, CommandText).ConfigureAwait(false);
			}
		}

		internal static async Task BotownerCommands(MessageCreateEventArgs Message_Objects) {
			Log.Debug("BotownerCommand check");
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string CommandPrefix = CommandObject.CommandArgs[0].Substring(0, CommandConfig.BotownerPrefix.Length);
			string CommandText = CommandObject.CommandArgs[0].Substring(CommandConfig.BotownerPrefix.Length);

			if (CommandPrefix != CommandConfig.BotownerPrefix) return;
			if (Message_Objects.Author.IsBot) return;
			if (!Message_Objects.Channel.IsPrivate) return;

			if (Message_Objects.Message.Author.Id != Configs.ClientConfig.BotownerId) {
				Log.Info("Not bot owner user tried to access the bot owner command.");
				return;
			}

			await ExcuteCommands<BotownerCommands>(CommandObject, CommandText).ConfigureAwait(false);
		}
		#endregion
	}
}
