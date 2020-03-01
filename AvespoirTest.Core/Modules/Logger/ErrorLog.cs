using DSharpPlus;
using System;
using static AvespoirTest.Core.Modules.Logger.LoggerProperties;

namespace AvespoirTest.Core.Modules.Logger {

	class ErrorLog {

		internal ErrorLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Error, Username, Message, DateTime.Now);
		}

		internal ErrorLog(Exception Error) {
			#if DEBUG
			string Message = Error.ToString();
			#else
			string Message = $"{Error.GetType()}: {Error.Message}";
			#endif

			Debug_Logger.LogMessage(LogLevel.Error, Username, Message, DateTime.Now);
		}
	}
}
