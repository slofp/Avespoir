using DSharpPlus.EventArgs;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Logger {

	class HeartbeatLog {

		internal static async Task ExportHeartbeatLog(HeartbeatEventArgs HeartbeatObjects) {
			string Ping = HeartbeatObjects.Ping.ToString();
			await Task.Run(() => new InfoLog($"Ping: {Ping}ms"));
		}
	}
}
