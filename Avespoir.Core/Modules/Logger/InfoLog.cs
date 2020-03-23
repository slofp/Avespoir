using DSharpPlus;
using System;
using static Avespoir.Core.Modules.Logger.LoggerProperties;

namespace Avespoir.Core.Modules.Logger {

	class InfoLog {

		internal InfoLog(string Message) {
			Debug_Logger.LogMessage(LogLevel.Info, Username, Message, DateTime.Now);
		}
	}
}
