using DSharpPlus;
using System;

namespace AvespoirTest.Core.Modules.Logger {

	class DebugLog {

		internal DebugLog(string Message) {
			#if DEBUG 
			LoggerProperties.Debug_Logger.LogMessage(LogLevel.Debug, LoggerProperties.Username, Message, DateTime.Now);
			#endif
		}
	}
}
