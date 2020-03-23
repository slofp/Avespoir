using DSharpPlus;
using System;
using static Avespoir.Core.Modules.Logger.LoggerProperties;

namespace Avespoir.Core.Modules.Logger {

	class DebugLog {

		internal DebugLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Debug, Username, Message, DateTime.Now);
		}
	}
}
