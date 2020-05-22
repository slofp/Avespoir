using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Logger {

	class HeartbeatLog {

		internal static Task ExportHeartbeatLog(HeartbeatEventArgs HeartbeatObjects) {
			string Ping = HeartbeatObjects.Ping.ToString();
			Log.Info($"Ping: {Ping}ms");

			return Task.CompletedTask;
		}
	}
}
