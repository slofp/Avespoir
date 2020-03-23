using DSharpPlus;
using System;
using static Avespoir.Core.Modules.Logger.LoggerProperties;

namespace Avespoir.Core.Modules.Logger {

	class WarningLog {

		internal WarningLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Warning, Username, Message, DateTime.Now);
		}
	}
}
