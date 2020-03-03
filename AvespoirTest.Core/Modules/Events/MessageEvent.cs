using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class MessageEvent {

		internal static async Task Main(MessageCreateEventArgs Message_Objects) {
			new DebugLog("MessageEvent " + "Start...");
			
			await Task.Run(() => new MessageLog(Message_Objects));

			//CommandRegister.PublicCommands(Message_Objects);

			//CommandRegister.ModeratorCommands(Message_Objects);

			//CommandRegister.BotownerCommands(Message_Objects);

			new DebugLog("MessageEvent " + "End...");
		}
	}
}
