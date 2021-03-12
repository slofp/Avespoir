using Avespoir.Core.Database;
using Avespoir.Core.Modules.Logger;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ConsoleExitEvent {

		internal static void Main(object Sender, ConsoleCancelEventArgs Args) => Exit(true).ConfigureAwait(false).GetAwaiter().GetResult();

		internal static void Main(object Sender, EventArgs Args) => Exit(false).ConfigureAwait(false).GetAwaiter().GetResult();

		internal static async Task Main(bool Stop) => await Exit(Stop).ConfigureAwait(false);

		private static bool AlreadyExiting = false;

		static async Task Exit(bool CancelKeyPress) {
			if (AlreadyExiting) {
				Log.Info("Already exiting.");
				return;
			}
			else AlreadyExiting = true;

			Log.Info("Exit...");

			ReadyEvent.ExitCheck = true;

			await Client.Bot.DisconnectAsync().ConfigureAwait(false);
			Log.Info("Bot Disconnected.");

			Client.Bot.Dispose();
			Log.Info("Bot Disposed.");

			LiteDBClient.DeleteDBAccess();
			Log.Info("Database Disconnected.");

			if (CancelKeyPress) Environment.Exit(Environment.ExitCode);
		}
	}
}
