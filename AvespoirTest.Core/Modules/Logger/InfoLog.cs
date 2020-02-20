using DSharpPlus;
using System;

namespace AvespoirTest.Core.Modules.Logger {

	class InfoLog {

		internal InfoLog(string Message) {
			LoggerProperties.Debug_Logger.LogMessage(LogLevel.Info, LoggerProperties.Username, Message, DateTime.Now);
		}
	}
}
