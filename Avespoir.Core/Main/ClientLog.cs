using Avespoir.Core.Exceptions;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Avespoir.Core {

	class ClientLog {

		static DiscordClient Bot = Client.Bot;

		static string LogDate = new DateTime(DateTime.Now.Ticks).ToString("yyyy-MM-dd_HH.mm.ss");

		static string LogDirPath = @"./Log";

		static string LogFilePath = $@"{LogDirPath}/{LogDate}.log";

		#nullable enable
		internal static void ExportLog(object? Sender, DebugLogMessageEventArgs Log) {
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
			Console.Clear();
			string ConsoleString = "Log File Creating.";
			Console.SetCursorPosition(0,0);
			Console.WriteLine(ConsoleString);

			FileStream LogFile;
			DirectoryInfo LogDirInfo;
			FileInfo LogFileInfo;

			try {
				Console.SetCursorPosition(ConsoleString.Length, 0);
				Console.WriteLine(".");

				LogDirInfo = new DirectoryInfo(LogDirPath);
				if (!LogDirInfo.Exists) {
					new WarningLog("Log Directory is not found. Createing Log Directory.");

					await Task.Factory.StartNew(() => LogDirInfo.Create()).ConfigureAwait(false);

					LogDirInfo.Refresh();
					if (!LogDirInfo.Exists) throw new DirectoryCouldNotCreatedException("Log directory could not created.");
				}

				Console.SetCursorPosition(ConsoleString.Length + 1, 0);
				Console.WriteLine(".");

				LogFileInfo = new FileInfo(LogFilePath);
				if (!LogFileInfo.Exists) {
					LogFile = await Task.Factory.StartNew(() => LogFileInfo.Create()).ConfigureAwait(false);
					LogFile.Dispose();

					LogFileInfo.Refresh();
					if (!LogFileInfo.Exists) throw new FileCouldNotCreatedException("Log file could not created.");
				}
				else {
					new WarningLog("Log file you are trying to create exist. This is automatically overwritten.");
					await Task.Factory.StartNew(() => LogFileInfo.Delete()).ConfigureAwait(false);
					
					LogFile = await Task.Factory.StartNew(() => LogFileInfo.Create()).ConfigureAwait(false);
					LogFile.Dispose();

					LogFileInfo.Refresh();
					if (!LogFileInfo.Exists) throw new FileCouldNotCreatedException("Log file could not created.");
				}

				Console.SetCursorPosition(ConsoleString.Length + 2, 0);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("OK");
				Console.ForegroundColor = ConsoleColor.White;
			}
			catch (Exception Error) {
				new ErrorLog(Error);
			}
		}

		
	}
}
