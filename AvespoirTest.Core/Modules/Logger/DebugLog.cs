using DSharpPlus;
using System;
using static AvespoirTest.Core.Modules.Logger.LoggerProperties;

namespace AvespoirTest.Core.Modules.Logger {

	class DebugLog {

		internal DebugLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Debug, Username, Message, DateTime.Now);
		}
	}
}
