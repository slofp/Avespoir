using DSharpPlus;
using System;
using static Avespoir.Core.Modules.Logger.LoggerProperties;

namespace Avespoir.Core.Modules.Logger {

	class CriticalLog {

		internal CriticalLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Critical, Username, Message, DateTime.Now);
		}
	}
}
