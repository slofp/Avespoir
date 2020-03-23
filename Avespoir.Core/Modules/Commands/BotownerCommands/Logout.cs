using Avespoir.Core.Attributes;
using Avespoir.Core.Modules.Logger;
using System.Threading.Tasks;
using System;

namespace Avespoir.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("logout")]
		public async Task Logout(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("終了します...");
			new InfoLog("Logging out...");

			await Client.Bot.DisconnectAsync();
			Client.Bot.Dispose();

			Environment.Exit(Environment.ExitCode);
		}
	}
}
