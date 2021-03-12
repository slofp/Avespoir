using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Configs;
using Avespoir.Core.Database.Enums;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	delegate Task CommandDelegate(CommandObjects CommandObject);

	class CommandRegister {

		private static readonly Assembly Assembly_Info = Assembly.GetExecutingAssembly();

		private static readonly Type[] Assembly_Types = Assembly_Info.GetTypes();

		private static readonly IEnumerable<Type> Commands_Types =
				from Assembly_Type in Assembly_Types
				where Assembly_Type.Namespace.Contains(new CommandRegister().GetType().Namespace)
				select Assembly_Type;

		private static async Task ExcuteCommands(CommandObjects CommandObject, string CommandText, RoleLevel Role_Level) {
			foreach (Type Commands_Type in Commands_Types) {
				if (!(Activator.CreateInstance(Commands_Type) is CommandAbstruct Command)) continue;

				CommandAttribute Command_Attribute = Commands_Type.GetCustomAttribute<CommandAttribute>();
				if (Command_Attribute is null) continue;

				if (Command_Attribute.CommandName == CommandText && Command_Attribute.CommandRoleLevel == Role_Level) {
					await Command.Excute(CommandObject).ContinueWith(CommandTask => {
						if (CommandTask.IsFaulted || CommandTask.IsCanceled) Log.Error(CommandTask.Exception);
					}).ConfigureAwait(false);

					return;
				}
			}

			Log.Error("Commnad Not Found.");
		}

		private static async Task ExcuteCommands<CommandsClass>(CommandObjects CommandObject, string CommandText) where CommandsClass : class {
			CommandDelegate Command_Delegate = null;
			Type CommandsType = typeof(CommandsClass);

			MethodInfo[] CommandsMethodInfo = CommandsType.GetMethods();
			foreach(MethodInfo CommandMethodInfo in CommandsMethodInfo) {
				Attribute GetCommandAttribute = Attribute.GetCustomAttribute(CommandMethodInfo, typeof(CommandAttribute));

				if (!(GetCommandAttribute is CommandAttribute Command_Attribute)) {
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

			string GuildPublicPrefix = Database.DatabaseMethods.GuildConfigMethods.PublicPrefixFind(CommandObject.Guild.Id);
			if (GuildPublicPrefix == null) GuildPublicPrefix = CommandConfig.PublicPrefix;

			string CommandPrefix = CommandObject.CommandArgs[0][0..GuildPublicPrefix.Length];
			string CommandText = CommandObject.CommandArgs[0][GuildPublicPrefix.Length..];

			if (CommandPrefix != GuildPublicPrefix) return;
			if (Message_Objects.Author.IsBot) return;

			await ExcuteCommands<PublicCommands>(CommandObject, CommandText).ConfigureAwait(false);

			Log.Debug("PublicCommand OK");
		}

		internal static async Task ModeratorCommands(MessageCreateEventArgs Message_Objects) {
			Log.Debug("ModeratorCommand check");
			if (Message_Objects.Channel.IsPrivate) return;
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string GuildModeratorPrefix = Database.DatabaseMethods.GuildConfigMethods.ModeratorPrefixFind(CommandObject.Guild.Id);
			if (GuildModeratorPrefix == null) GuildModeratorPrefix = CommandConfig.ModeratorPrefix;

			string CommandPrefix = CommandObject.CommandArgs[0][0..GuildModeratorPrefix.Length];
			string CommandText = CommandObject.CommandArgs[0][GuildModeratorPrefix.Length..];

			if (CommandPrefix != GuildModeratorPrefix) return;
			if (Message_Objects.Author.IsBot) return;

			if (Message_Objects.Message.Author.Id == Message_Objects.Guild.Owner.Id) {
				await ExcuteCommands<ModeratorCommands>(CommandObject, CommandText).ConfigureAwait(false);
			}
			else {
				bool ModCheck = false;

				Database.DatabaseMethods.RolesMethods.RolesListFind(Message_Objects.Guild.Id, RoleLevel.Moderator, out List<Roles> DBRoleList);
				foreach (Roles DBRole in DBRoleList) {
					DiscordMember GuildMember = await Message_Objects.Guild.GetMemberAsync(Message_Objects.Message.Author.Id);

					List<DiscordRole> GuildRoleList = GuildMember.Roles.ToList();
					foreach (DiscordRole GuildRole in GuildRoleList) {
						if (GuildRole.Id == DBRole.Uuid) {
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

			string CommandPrefix = CommandObject.CommandArgs[0][0..CommandConfig.BotownerPrefix.Length];
			string CommandText = CommandObject.CommandArgs[0][CommandConfig.BotownerPrefix.Length..];

			if (CommandPrefix != CommandConfig.BotownerPrefix) return;
			if (Message_Objects.Author.IsBot) return;
			if (!Message_Objects.Channel.IsPrivate) return;

			if (Message_Objects.Message.Author.Id != ClientConfig.BotownerId) {
				Log.Info("Not bot owner user tried to access the bot owner command.");
				return;
			}

			await ExcuteCommands<BotownerCommands>(CommandObject, CommandText).ConfigureAwait(false);
		}
		#endregion

		private enum PrefixType {
			Public,
			Moderator,
			Owner
		}

		private static void PrefixCheck() {
			
		}
	}
}
