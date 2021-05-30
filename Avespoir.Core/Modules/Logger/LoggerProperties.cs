using DSharpPlus;
using log4net;
using log4net.Repository;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Avespoir.Core.Modules.Logger {

	class LoggerProperties {

		static readonly ILoggerRepository Repository = LogManager.CreateRepository("default");

		static ILog Log_ {
			get {
				return LogManager.GetLogger("default", Username);
			}
		}

		internal static ILog Log {
			get {
				using Stream ConfigData = Assembly.GetExecutingAssembly().GetManifestResourceStream("Avespoir.Core.Modules.Logger.log4net.config");

				if (ConfigData != null) log4net.Config.XmlConfigurator.Configure(Repository, ConfigData);
				return Log_;
			}
		}
		
		internal static string Username {
			get {
				if (string.IsNullOrWhiteSpace(Client.Bot.CurrentUser?.Username)) return "Bot";
				else return Client.Bot.CurrentUser.Username;
			}
		}
	}
}
