using Avespoir.Core.Attributes;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Commands {

	partial class PublicCommands {

		[Command("ver")]
		public async Task Version(CommandObjects CommandObject) {
			string VersionString = string.Format("BotName: {0}\nBotVersion: {1}", Client.Bot.CurrentUser.Username, Client.Version);
			await CommandObject.Message.Channel.SendMessageAsync(VersionString);
		}
	}
}
