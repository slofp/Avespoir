using AvespoirTest.Core.Exceptions;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	class ClientLog {

		DiscordClient Bot = Client.Bot;

		static string LogDate = new DateTime(DateTime.Now.Ticks).ToString("yyyy-MM-dd_HH.mm.ss");

		static string LogDirPath = @"./Log";

		static string LogFilePath = $@"{LogDirPath}/{LogDate}.log";

		internal ClientLog() {
			ReleaseLog();
			ExportLog();
		}

		[Conditional("RELEASE")]
		void ReleaseLog() {
			Bot.DebugLogger.LogMessageReceived += (Sender, Log) => Console.WriteLine(Log);
			Bot.Heartbeated += HeartbeatObjects => HeartbeatLog.ExportHeartbeatLog(HeartbeatObjects);
		}

		void ExportLog() {
			Bot.DebugLogger.LogMessageReceived += (Sender, Log) => {
				FileStream LogFile;
				StreamWriter LogWriter;
				FileInfo LogFileInfo;

				try {
					LogFileInfo = new FileInfo(LogFilePath);
					LogFile = LogFileInfo.Open(FileMode.Append, FileAccess.Write);
					LogWriter = new StreamWriter(LogFile);
					
					LogWriter.WriteLine(Log);
					LogWriter.Dispose();

					LogFile.Dispose();
				}
				catch (Exception Error) {
					new ErrorLog(Error);
				}
			};
		}

		internal static async Task InitlogFile() {
			FileStream LogFile;
			DirectoryInfo LogDirInfo;
			FileInfo LogFileInfo;

			try {
				LogDirInfo = new DirectoryInfo(LogDirPath);
				if (!LogDirInfo.Exists) {
					new WarningLog("Log Directory is not found. Createing Log Directory.");

					await Task.Run(() => LogDirInfo.Create());

					LogDirInfo.Refresh();
					if (!LogDirInfo.Exists) throw new DirectoryCouldNotCreatedException("Log directory could not created.");
				}

				LogFileInfo = new FileInfo(LogFilePath);
				if (!LogFileInfo.Exists) {
					LogFile = await Task.Run(() => LogFileInfo.Create());
					LogFile.Dispose();

					LogFileInfo.Refresh();
					if (!LogFileInfo.Exists) throw new FileCouldNotCreatedException("Log file could not created.");
				}
				else {
					new WarningLog("Log file you are trying to create exist. This is automatically overwritten.");
					await Task.Run(() => LogFileInfo.Delete());
					
					LogFile = await Task.Run(() => LogFileInfo.Create());
					LogFile.Dispose();

					LogFileInfo.Refresh();
					if (!LogFileInfo.Exists) throw new FileCouldNotCreatedException("Log file could not created.");
				}

			}
			catch (Exception Error) {
				new ErrorLog(Error);
			}
		}
	}
}
