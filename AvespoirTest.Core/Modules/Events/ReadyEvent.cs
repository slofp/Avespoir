using AvespoirTest.Core.Database;
using AvespoirTest.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Modules.Events {

	class ReadyEvent {

		static DiscordGame StartingStatus = new DiscordGame() {
			Name = "Starting...",
		};

		static DiscordGame ReadyStatus = new DiscordGame() {
			Name = "Bot Ready!",
		};

		internal static async Task Main(ReadyEventArgs ReadyEventObjects) {
			new DebugLog("ReadyEvent " + "Start...");
			BaseDiscordClient ClientBase = ReadyEventObjects.Client;
			await Client.Bot.UpdateStatusAsync(StartingStatus, UserStatus.DoNotDisturb);

			await Client.Bot.UpdateStatusAsync(ReadyStatus, UserStatus.Online);

			new InfoLog($"{ClientBase.CurrentUser.Username} Bot Ready!");
			await Task.Delay(5000).ConfigureAwait(false);

			await Client.Bot.UpdateStatusAsync(null, UserStatus.Online);
			new DebugLog("ReadyEvent " + "End...");
		}
	}
}
