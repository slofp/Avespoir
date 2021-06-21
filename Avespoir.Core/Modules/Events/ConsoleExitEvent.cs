using Avespoir.Core.Database;
using Avespoir.Core.Modules.Logger;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ConsoleExitEvent {

		internal static void Main(object Sender, ConsoleCancelEventArgs Args) => Exit(0).ConfigureAwait(false).GetAwaiter().GetResult();

		internal static void Main(object Sender, EventArgs Args) => Exit(1).ConfigureAwait(false).GetAwaiter().GetResult();

		internal static async Task Main(int ExitCode) => await Exit(ExitCode).ConfigureAwait(false);

		private static bool AlreadyExiting = false;

		static async Task Exit(int ExitCode) {
			if (AlreadyExiting) {
				Log.Info("Already exiting.");
				return;
			}
			else AlreadyExiting = true;

			ReadyEvent.ExitCheck = true;

			await Client.Bot.StopAsync().ConfigureAwait(false);
			Log.Info("Bot Stopped.");

			await Client.Bot.LogoutAsync().ConfigureAwait(false);

			Client.Bot.Dispose();
			Log.Info("Bot Disposed.");

			LiteDBClient.DeleteDBAccess();
			Log.Info("Database Disconnected.");

			if (ExitCode <= 0) {
				if (ExitCode == -1) {
					string ProgramPath = string.Format("{0}Avespoir", AppDomain.CurrentDomain.BaseDirectory);

					try {
						Process.Start(ProgramPath);
					}
					catch (System.ComponentModel.Win32Exception e) {
						Log.Debug(ProgramPath);
						Log.Error("Restart Error", e);
					}
				}

				Log.Info("Exit...");
				Environment.Exit(Environment.ExitCode);
			}
		}
	}
}
