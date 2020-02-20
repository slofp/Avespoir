using AvespoirTest.Core.Modules.Logger;
using DSharpPlus;
using System.Threading.Tasks;
using System;

namespace AvespoirTest.Core {

	class ClientLog {

		DiscordClient Bot = Client.Bot;

		// Release only
		internal ClientLog() {
			#if !DEBUG
			Bot.DebugLogger.LogMessageReceived += (Sender, Log) => Console.WriteLine(Log);
			Bot.Heartbeated += HeartbeatObjects => Task.Run(() => new HeartbeatLog(HeartbeatObjects));
			#endif
		}
	}
}
