using AvespoirTest.Core.Modules.Logger;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class GuildMemberAddEvent {

		internal static async Task Main(GuildMemberAddEventArgs MemberObjects) {
			new DebugLog("GuildMemberAddEvent " + "Start...");
			await Task.Delay(0);
			new DebugLog("GuildMemberAddEvent " + "End...");
		}
	}
}
