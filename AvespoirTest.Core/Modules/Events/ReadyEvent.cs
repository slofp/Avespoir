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

			for (int retry = 0; retry < 6; retry++) {
				if (MongoDBClient.Connectcheck) {
					IAsyncCursor<string> DBCol = MongoDBClient.Database.ListCollectionNames();
					List<string> DbColList = await DBCol.ToListAsync();
					DbColList.ForEach(col => {
						Console.WriteLine(col);
					});
				}
				else {
					await Task.Delay(5000).ConfigureAwait(false);
					if (retry == 5) new WarningLog("ReadyEvent " + "Process could not performed because there is not connect to database.");
					else {
						new DebugLog("ReadyEvent " + $"Retrying... ({5 - (retry + 1)} times until the maximum number of retries.)");
						continue;
					}
				}
			}

			await Client.Bot.UpdateStatusAsync(ReadyStatus, UserStatus.Online);

			new InfoLog($"{ClientBase.CurrentUser.Username} Bot Ready!");
			await Task.Delay(5000).ConfigureAwait(false);

			await Client.Bot.UpdateStatusAsync(null, UserStatus.Online);
			new DebugLog("ReadyEvent " + "End...");
		}
	}
}
