using DSharpPlus;
using System;
using static AvespoirTest.Core.Modules.Logger.LoggerProperties;

namespace AvespoirTest.Core.Modules.Logger {

	class CriticalLog {

		internal CriticalLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Critical, Username, Message, DateTime.Now);
		}
	}
}
