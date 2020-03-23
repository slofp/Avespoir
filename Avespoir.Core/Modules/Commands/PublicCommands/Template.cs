using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Logger;
using Avespoir.Core.Modules.Utils;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

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
