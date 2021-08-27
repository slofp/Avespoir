using System;
using System.Diagnostics;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		[Conditional("DEBUG")]
		internal static void Debug(object Message) => LoggerProperties.Log.Debug(Message);

		[Conditional("DEBUG")]
		internal static void Debug(object Message, Exception Error) => LoggerProperties.Log.Debug(Message, Error);
	}
}
