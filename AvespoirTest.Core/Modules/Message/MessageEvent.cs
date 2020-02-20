using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Message {

	class MessageEvent {

		internal static async Task MainEvent(MessageCreateEventArgs Message_Objects) {
			await Task.Run(() => new MessageLog(Message_Objects));
		}
	}
}
