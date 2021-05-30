using Avespoir.Core.Abstructs;
using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Avespoir.Core.Modules.Commands {

	class CommandInfo {

		private static readonly Assembly Assembly_Info = Assembly.GetExecutingAssembly();

		private static readonly Type[] Assembly_Types = Assembly_Info.GetTypes();

		private static readonly string CommandRegisterNamespace = new CommandRegister().GetType().Namespace;

		private static readonly IEnumerable<Type> Commands_Types =
				from Assembly_Type in Assembly_Types
				where !(Assembly_Type.Namespace is null) && Assembly_Type.Namespace.Contains(CommandRegisterNamespace)
				select Assembly_Type;

		internal CommandAttribute Command_Attribute { get; private set; }

		internal CommandAbstruct Command => LazyCommand.Value;

		private Lazy<CommandAbstruct> LazyCommand;

		private CommandInfo() { }

		internal static IEnumerable<CommandInfo> GetCommandInfo() {
			List<CommandInfo> CommandInfoList = new List<CommandInfo>();

			foreach (Type Commands_Type in Commands_Types) {
				CommandInfo Command_Info = new CommandInfo {
					Command_Attribute = Commands_Type.GetCustomAttribute<CommandAttribute>(),

					LazyCommand = new Lazy<CommandAbstruct>(() => Activator.CreateInstance(Commands_Type) as CommandAbstruct)
				};

				if (Command_Info.Command_Attribute is null) continue;
				/*if (Command_Info.Command is null) {
					Log.Error("Command is not wrapped with CommandAbstruct.");
					continue;
				}*/

				CommandInfoList.Add(Command_Info);
			}

			return CommandInfoList.AsEnumerable();
		}
	}
}
