using System;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Info(object Message) => LoggerProperties.Log.Info(Message);

		internal static void Info(object Message, Exception Error) => LoggerProperties.Log.Info(Message, Error);
	}
}
