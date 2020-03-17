using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Modules.Logger;
using AvespoirTest.Core.Modules.Utils;
using System;
using System.Threading.Tasks;

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
