using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("restart")]
		public async Task Restart(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("Restarting...");
			Log.Info("Restarting...");

			await ConsoleExitEvent.Main(false).ConfigureAwait(false);

			Process.Start(Assembly.GetEntryAssembly().Location);

			Environment.Exit(Environment.ExitCode);
		}
	}
}
