using AvespoirTest.Core.Exceptions;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AvespoirTest.Core {

	class ClientLog {

		static DiscordClient Bot = Client.Bot;

		static string LogDate = new DateTime(DateTime.Now.Ticks).ToString("yyyy-MM-dd_HH.mm.ss");

		static string LogDirPath = @"./Log";

		static string LogFilePath = $@"{LogDirPath}/{LogDate}.log";

		internal ClientLog() {}
		
		internal void StartClientLogEvents() {
			#if !DEBUG
			Bot.DebugLogger.LogMessageReceived += (Sender, Log) => Console.WriteLine(Log);
			Bot.Heartbeated += HeartbeatLog.ExportHeartbeatLog;
			#endif
			Bot.DebugLogger.LogMessageReceived += ExportLog;
		}

		#nullable enable
		internal void ExportLog(object? Sender, DebugLogMessageEventArgs Log) {
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
		}

		internal static async Task InitlogFile() {
			FileStream LogFile;
			DirectoryInfo LogDirInfo;
			FileInfo LogFileInfo;

			try {
				LogDirInfo = new DirectoryInfo(LogDirPath);
				if (!LogDirInfo.Exists) {
					new WarningLog("Log Directory is not found. Createing Log Directory.");

					await Task.Factory.StartNew(() => LogDirInfo.Create()).ConfigureAwait(false);

					LogDirInfo.Refresh();
					if (!LogDirInfo.Exists) throw new DirectoryCouldNotCreatedException("Log directory could not created.");
				}

				LogFileInfo = new FileInfo(LogFilePath);
				if (!LogFileInfo.Exists) {
					LogFile = await Task.Factory.StartNew(() => LogFileInfo.Create()).ConfigureAwait(false);
					LogFile.Dispose();

					LogFileInfo.Refresh();
					if (!LogFileInfo.Exists) throw new FileCouldNotCreatedException("Log file could not created.");
				}
				else {
					new WarningLog("Log file you are trying to create exist. This is automatically overwritten.");
					await Task.Run(() => LogFileInfo.Delete());
					
					LogFile = await Task.Factory.StartNew(() => LogFileInfo.Create()).ConfigureAwait(false);
					LogFile.Dispose();

					LogFileInfo.Refresh();
					if (!LogFileInfo.Exists) throw new FileCouldNotCreatedException("Log file could not created.");
				}
				Console.WriteLine("OK");
			}
			catch (Exception Error) {
				new ErrorLog(Error);
			}
		}

		
	}
}
