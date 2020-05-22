using DSharpPlus;
using log4net;
using log4net.Repository;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Avespoir.Core.Modules.Logger {

	class LoggerProperties {

		static ILoggerRepository Repository = LogManager.CreateRepository("default");

		static ILog Log_ {
			get {
				return LogManager.GetLogger("default", Username);
			}
		}

		internal static ILog Log {
			get {
				Assembly GetAssembly = Assembly.GetExecutingAssembly();
				string ConfigName = GetAssembly.GetManifes‌​tResourceNames().FirstOrDefault(ListStr => ListStr.EndsWith("log4net.config"));
				if (ConfigName != null) {
					using Stream ConfigData = GetAssembly.GetManifestResourceStream(ConfigName);
					log4net.Config.XmlConfigurator.Configure(Repository, ConfigData);
				}
				return Log_;
			}
		}
		
		internal static string Username {
			get {
				try {
					if (string.IsNullOrWhiteSpace(Client.Bot.CurrentUser.Username)) return "Bot";
					else return Client.Bot.CurrentUser.Username;
				}
				catch (NullReferenceException) {
					return "Bot";
				}
			}
		}
	}
}
