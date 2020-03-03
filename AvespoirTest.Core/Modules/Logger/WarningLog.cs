using DSharpPlus;
using System;
using static AvespoirTest.Core.Modules.Logger.LoggerProperties;

namespace AvespoirTest.Core.Modules.Logger {

	class WarningLog {

		internal WarningLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Warning, Username, Message, DateTime.Now);
		}
	}
}
