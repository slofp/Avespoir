using Avespoir.Core.Modules.Logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avespoir.Core.Modules.Events {

	class ConsoleExitEvent {

		internal static void Main(object Sender, ConsoleCancelEventArgs Args) {
			Log.Info("Exit...");

			Client.Bot.DisconnectAsync().ConfigureAwait(false).GetAwaiter().GetResult();
			Client.Bot.Dispose();

			Environment.Exit(Environment.ExitCode);
		}
	}
}
