using System.Diagnostics;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		[Conditional("DEBUG")]
		internal static void Debug(object Message) => LoggerProperties.Log.Debug(Message);
	}
}
