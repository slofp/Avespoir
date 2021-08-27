using log4net.Core;
using System;
using System.Reflection;

namespace Avespoir.Core.Modules.Logger {

	partial class Log {

		internal static void Verbose(string Message) =>
			Verbose(Message, null);

		internal static void Verbose(string Message, Exception Error) =>
			LoggerProperties.Log.Logger.Log(MethodBase.GetCurrentMethod().DeclaringType, Level.Verbose, Message, Error);
	}
}
