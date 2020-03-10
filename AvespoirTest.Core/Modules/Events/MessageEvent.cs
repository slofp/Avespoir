using AvespoirTest.Core.Modules.Commands;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class MessageEvent {

		internal static async Task Main(MessageCreateEventArgs Message_Objects) {
			new DebugLog("MessageEvent " + "Start...");
			
			await Task.Run(() => new MessageLog(Message_Objects));

			Task PublicCommandTask = CommandRegister.PublicCommands(Message_Objects);

			Task ModeratorCommandTask = CommandRegister.ModeratorCommands(Message_Objects);

			Task BotownerCommandTask = CommandRegister.BotownerCommands(Message_Objects);

			await Task.WhenAll(PublicCommandTask, ModeratorCommandTask, BotownerCommandTask);

			new DebugLog("MessageEvent " + "End...");
		}
	}
}
