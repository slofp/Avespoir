using System;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Error(object Message) => LoggerProperties.Log.Error(Message);

		internal static void Error(object Message, Exception Error) => LoggerProperties.Log.Error(Message, Error);
	}
}
