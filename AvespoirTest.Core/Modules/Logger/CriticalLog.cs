using DSharpPlus;
using System;

namespace AvespoirTest.Core.Modules.Logger {

	class CriticalLog {

		internal CriticalLog(string Message) {
			LoggerProperties.Debug_Logger.LogMessage(LogLevel.Critical, LoggerProperties.Username, Message, DateTime.Now);
		}
	}
}
