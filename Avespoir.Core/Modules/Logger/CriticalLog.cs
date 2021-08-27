using System;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Critical(object Message) => LoggerProperties.Log.Fatal(Message);

		internal static void Critical(object Message, Exception Error) => LoggerProperties.Log.Fatal(Message, Error);
	}
}
