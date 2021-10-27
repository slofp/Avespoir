using Microsoft.Extensions.Logging;

namespace Avespoir.Core.Modules.Logger {

	class Log4NetProvider : ILoggerProvider {

		public ILogger CreateLogger(string _) => Log4NetLogger.LoggerModule;

		public void Dispose() { }
	}
}
