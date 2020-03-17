using AvespoirTest.Core.Modules.Commands;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class MessageEvent {

		internal static Task Main(MessageCreateEventArgs Message_Objects) {
			new DebugLog("MessageEvent " + "Start...");
			
			Task.Factory.StartNew(() => new MessageLog(Message_Objects)).ConfigureAwait(false);

			CommandRegister.PublicCommands(Message_Objects).ConfigureAwait(false);

			CommandRegister.ModeratorCommands(Message_Objects).ConfigureAwait(false);

			CommandRegister.BotownerCommands(Message_Objects).ConfigureAwait(false);

			new DebugLog("MessageEvent " + "End...");

			return Task.CompletedTask;
		}
	}
}
