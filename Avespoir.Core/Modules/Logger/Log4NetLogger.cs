using Microsoft.Extensions.Logging;
using System;

namespace Avespoir.Core.Modules.Logger {

	public class Log4NetLogger : ILogger {

		internal static Log4NetLogger LoggerModule = new Log4NetLogger();

		public IDisposable BeginScope<TState>(TState _) => null;

		public bool IsEnabled(LogLevel logLevel) {
			switch (logLevel) {
				case LogLevel.Critical:
					return LoggerProperties.Log.IsFatalEnabled;
				case LogLevel.Trace:
				case LogLevel.Debug:
					return LoggerProperties.Log.IsDebugEnabled;
				case LogLevel.Error:
					return LoggerProperties.Log.IsErrorEnabled;
				case LogLevel.Information:
					return LoggerProperties.Log.IsInfoEnabled;
				case LogLevel.Warning:
					return LoggerProperties.Log.IsWarnEnabled;
				default:
					throw new ArgumentException(nameof(logLevel));
			}
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) {
			if (!IsEnabled(logLevel)) return;
			if (formatter == null) throw new ArgumentNullException(nameof(formatter));

			string LogMessage = formatter(state, exception);
			if (string.IsNullOrWhiteSpace(LogMessage)) return;

			switch (logLevel) {
				case LogLevel.Critical:
					Logger.Log.Critical(LogMessage);
					break;
				case LogLevel.Trace:
				case LogLevel.Debug:
					Logger.Log.Debug(LogMessage);
					break;
				case LogLevel.Error:
					Logger.Log.Error(LogMessage);
					break;
				case LogLevel.Information:
					Logger.Log.Info(LogMessage);
					break;
				case LogLevel.Warning:
					Logger.Log.Warning(LogMessage);
					break;
				default:
					Logger.Log.Warning($"{logLevel} is unknown. write the message as info level.");
					Logger.Log.Info(LogMessage);
					break;
			}
		}
	}
}
