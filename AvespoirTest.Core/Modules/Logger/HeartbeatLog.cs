using DSharpPlus.EventArgs;

namespace AvespoirTest.Core.Modules.Logger {

	class HeartbeatLog {

		internal HeartbeatLog(HeartbeatEventArgs HeartbeatObjects) {
			string Ping = HeartbeatObjects.Ping.ToString();
			new InfoLog($"Ping: {Ping}ms");
		}
	}
}
