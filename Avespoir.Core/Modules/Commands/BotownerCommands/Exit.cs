using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Events;
using Avespoir.Core.Modules.Logger;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("exit")]
		public async Task Exit(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("Logging out...");
			Log.Info("Logging out...");

			await ConsoleExitEvent.Main(true).ConfigureAwait(false);
		}
	}
}
