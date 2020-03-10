using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class GuildMemberRemoveEvent {

		internal static async Task Main(GuildMemberRemoveEventArgs MemberObjects) {
			new DebugLog("GuildMemberRemoveEvent " + "Start...");
			await Task.Delay(0);
			new DebugLog("GuildMemberRemoveEvent " + "End...");
		}
	}
}
