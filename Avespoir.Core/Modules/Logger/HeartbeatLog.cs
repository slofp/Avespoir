using DSharpPlus;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Logger {

	class HeartbeatLog {

		internal static Task ExportHeartbeatLog(DiscordClient Bot, HeartbeatEventArgs HeartbeatObjects) {
			string Ping = HeartbeatObjects.Ping.ToString();
			Log.Info($"Ping: {Ping}ms");

			return Task.CompletedTask;
		}
	}
}
