using System;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Warning(object Message) => LoggerProperties.Log.Warn(Message);

		internal static void Warning(object Message, Exception Error) => LoggerProperties.Log.Warn(Message, Error);
	}
}
