using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Database;
using AvespoirTest.Core.Database.Schemas;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

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
					new ErrorLog("Return value is not regular expression.");
					continue;
				}

				Command_Delegate = Delegate.CreateDelegate(typeof(CommandDelegate), null, CommandMethodInfo) as CommandDelegate;
				break;
			}

			if (Command_Delegate != null) {
				await Command_Delegate.Invoke(CommandObject);
			}
			else {
				new ErrorLog("Commnad Not Found.");
			}
		}

		#region Command checker

		internal static async Task PublicCommands(MessageCreateEventArgs Message_Objects) {
			new DebugLog("PublicCommand check");
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string CommandPrefix = CommandObject.CommandArgs[0].Substring(0, CommandConfig.PublicPrefix.Length);
			string CommandText = CommandObject.CommandArgs[0].Substring(CommandConfig.PublicPrefix.Length);

			if (CommandPrefix != CommandConfig.PublicPrefix) return;
			if (Message_Objects.Author.IsBot) return;
			if (Message_Objects.Channel.IsPrivate) return;

			await ExcuteCommands<PublicCommands>(CommandObject, CommandText).ConfigureAwait(false);
		}

		internal static async Task ModeratorCommands(MessageCreateEventArgs Message_Objects) {
			new DebugLog("ModeratorCommand check");
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string CommandPrefix = CommandObject.CommandArgs[0].Substring(0, CommandConfig.ModeratorPrefix.Length);
			string CommandText = CommandObject.CommandArgs[0].Substring(CommandConfig.ModeratorPrefix.Length);

			if (CommandPrefix != CommandConfig.ModeratorPrefix) return;
			if (Message_Objects.Author.IsBot) return;
			if (Message_Objects.Channel.IsPrivate) return;

			

			await ExcuteCommands<ModeratorCommands>(CommandObject, CommandText).ConfigureAwait(false);
		}

		internal static async Task BotownerCommands(MessageCreateEventArgs Message_Objects) {
			new DebugLog("BotownerCommand check");
			CommandObjects CommandObject = new CommandObjects(Message_Objects);

			string CommandPrefix = CommandObject.CommandArgs[0].Substring(0, CommandConfig.BotownerPrefix.Length);
			string CommandText = CommandObject.CommandArgs[0].Substring(CommandConfig.BotownerPrefix.Length);

			if (CommandPrefix != CommandConfig.BotownerPrefix) return;
			if (Message_Objects.Author.IsBot) return;
			if (!Message_Objects.Channel.IsPrivate) return;

			await ExcuteCommands<BotownerCommands>(CommandObject, CommandText).ConfigureAwait(false);
		}
		#endregion
	}
}
