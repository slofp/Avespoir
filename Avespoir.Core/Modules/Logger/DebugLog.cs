namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Debug(object Message) {
			#if DEBUG
			LoggerProperties.Log.Debug(Message);
			#endif
		}
	}
}
