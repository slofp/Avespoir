using AvespoirTest.Core.Attributes;
using AvespoirTest.Core.Database;
using AvespoirTest.Core.Modules.Logger;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Commands {

	partial class BotownerCommands {

		[Command("restart")]
		public async Task Restart(CommandObjects CommandObject) {
			await CommandObject.Message.RespondAsync("再起動します...");
			new InfoLog("Restarting...");

			await Client.Bot.DisconnectAsync();

			Client.Bot.Dispose();
			MongoDBClient.DeleteDBAccess();

			await MongoDBClient.Main().ConfigureAwait(false);
			await Client.Bot.ConnectAsync();
		}
	}
}
