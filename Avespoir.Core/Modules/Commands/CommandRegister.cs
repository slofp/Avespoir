using Avespoir.Core.Configs;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Extends;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	class CommandRegister {

		private static async Task ExecuteCommands(CommandObject Command_Object, string CommandText, RoleLevel Role_Level) {
			Log.Debug($"Command RoleLevel [{Role_Level}]");

			if (Role_Level == RoleLevel.Bot) {
				Log.Error("Why this method access allowed?");
				return;
			}

			foreach (CommandInfo Command_Info in CommandInfo.GetCommandInfo()) {
				if (Command_Info.Command_Attribute.CommandName != CommandText) continue;
				else if (Command_Info.Command_Attribute.CommandRoleLevel == RoleLevel.Bot) {
					Log.Error("It will never be executed.");
					continue;
				}
				else if (Command_Info.Command_Attribute.CommandRoleLevel != RoleLevel.Owner && Command_Object.IsPrivate) continue;
				else if (Command_Info.Command_Attribute.CommandRoleLevel == RoleLevel.Owner && !Command_Object.IsPrivate) continue;

				if (Role_Level != RoleLevel.Owner) {
					if (Command_Info.Command_Attribute.CommandRoleLevel == RoleLevel.Owner) {
						Log.Info("Not bot owner user tried to access the bot owner command.");
						continue;
					}
					else if (Command_Info.Command_Attribute.CommandRoleLevel > Role_Level) {
						Log.Info("The member who has not been granted roles registered in the database tried to access the command.");
						continue;
					}
				}

				if (Command_Info.Command is null) {
					Log.Error("Command is not wrapped with CommandAbstruct.");
					continue;
				}

				await Command_Info.Command.Execute(Command_Object).ContinueWith(CommandTask => {
					if (CommandTask.IsFaulted || CommandTask.IsCanceled)
						Log.Error($"Command Execute Error in {Command_Info.Command_Attribute.CommandName}", CommandTask.Exception);
					else Log.Info($"{Command_Info.Command_Attribute.CommandName} completed successfully.");
				}).ConfigureAwait(false);

				return;
			}

			Log.Error("Commnad Not Found.");
		}

		#region Command checker

		internal static async Task Start(MessageObject Message_Object) {
			Log.Debug("Command check");

			CommandObject Command_Object = new CommandObject(Message_Object);

			// Bot Check
			if (Command_Object.Author.IsBot) return;
			//if (Message_Objects.Channel.IsPrivate) return;

			// Prefix Check

			string GuildPrefix = Command_Object.Guild is null ? null : Database.DatabaseMethods.GuildConfigMethods.PrefixFind(Command_Object.Guild.Id);
			if (GuildPrefix == null) GuildPrefix = CommandConfig.Prefix;

			string CommandPrefix = Command_Object.CommandArgs[0][0..GuildPrefix.Length];
			string CommandText = Command_Object.CommandArgs[0][GuildPrefix.Length..];

			if (CommandPrefix != GuildPrefix) return;

			// Level Check

			if (Command_Object.Author.Id == ClientConfig.BotownerId)
				await ExecuteCommands(Command_Object, CommandText, RoleLevel.Owner).ConfigureAwait(false);
			else if (Command_Object.Author.Id == Command_Object.Guild.Owner.Id)
				await ExecuteCommands(Command_Object, CommandText, RoleLevel.Moderator).ConfigureAwait(false);
			else {
				bool ModCheck = false;

				if (Database.DatabaseMethods.RolesMethods.RolesListFind(Command_Object.Guild.Id, RoleLevel.Moderator, out List<Roles> DBRoleList)) {
					foreach (Roles DBRole in DBRoleList) {
						foreach (DiscordRole GuildRole in Command_Object.Member.Roles) {
							if (GuildRole.Id == DBRole.Uuid) {
								ModCheck = true;
								break;
							}
							else continue;
						}

						if (ModCheck) break;
						else continue;
					}
				}

				if (ModCheck) await ExecuteCommands(Command_Object, CommandText, RoleLevel.Moderator).ConfigureAwait(false);
				else await ExecuteCommands(Command_Object, CommandText, RoleLevel.Public).ConfigureAwait(false);
			}

			Log.Debug("Command OK");
		}

		#endregion
	}
}
