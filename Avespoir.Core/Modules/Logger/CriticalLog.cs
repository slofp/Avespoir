namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Critical(object Message) => LoggerProperties.Log.Fatal(Message);
	}
}
