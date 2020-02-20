using DSharpPlus;
using System;

namespace AvespoirTest.Core.Modules.Logger {

	class ErrorLog {

		internal ErrorLog(string Message) {
			LoggerProperties.Debug_Logger.LogMessage(LogLevel.Error, LoggerProperties.Username, Message, DateTime.Now);
		}
	}
}
