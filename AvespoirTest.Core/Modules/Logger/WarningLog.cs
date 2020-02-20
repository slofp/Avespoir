using DSharpPlus;
using System;

namespace AvespoirTest.Core.Modules.Logger {

	class WarningLog {

		internal WarningLog(string Message) {
			LoggerProperties.Debug_Logger.LogMessage(LogLevel.Warning, LoggerProperties.Username, Message, DateTime.Now);
		}
	}
}
