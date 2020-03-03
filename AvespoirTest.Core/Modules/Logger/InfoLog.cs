using DSharpPlus;
using System;
using static AvespoirTest.Core.Modules.Logger.LoggerProperties;

namespace AvespoirTest.Core.Modules.Logger {

	class InfoLog {

		internal InfoLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Info, Username, Message, DateTime.Now);
		}
	}
}
