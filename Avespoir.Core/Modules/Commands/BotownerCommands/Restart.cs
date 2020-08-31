using Avespoir.Core.Attributes;
using Avespoir.Core.Database;
using Avespoir.Core.Modules.Logger;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("restart")]
		public async Task Restart(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("Restarting...");
			Log.Info("Restarting...");

			await Client.Bot.DisconnectAsync();

			Client.Bot.Dispose();
			MongoDBClient.DeleteDBAccess();

			await MongoDBClient.Main().ConfigureAwait(false);
			await Client.Bot.ConnectAsync();
		}
	}
}
