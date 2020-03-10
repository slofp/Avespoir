using AvespoirTest.Core.Attributes;
using System.Threading.Tasks;
using System;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Logger;
using AvespoirTest.Core.Exceptions;
using AvespoirTest.Core.Modules.Utils;

namespace AvespoirTest.Core.Modules.Commands {

	partial class PublicCommands {

		[Command()]
		public async Task Name(CommandObjects CommandObject) {
			try {
				string[] msgs = CommandObject.CommandArgs.Remove(0);
				await Task.Delay(0);
			}
			catch (Exception Error) {
				new ErrorLog(Error.Message);
			}
		}
	}
}
