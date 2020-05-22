﻿using Avespoir.Core.Configs;
using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.Events {

	class ReadyEvent {

		static DiscordGame StartingStatus = new DiscordGame() {
			Name = "Starting...",
		};

		internal static Task Main(ReadyEventArgs ReadyEventObjects) {
			Log.Debug("ReadyEvent " + "Start...");
			BaseDiscordClient ClientBase = ReadyEventObjects.Client;
			Client.Bot.UpdateStatusAsync(StartingStatus, UserStatus.DoNotDisturb).ConfigureAwait(false);

			Log.Info($"{ClientBase.CurrentUser.Username} Bot Ready!");

			DBCount().ConfigureAwait(false);
			Log.Debug("ReadyEvent " + "End...");
			return Task.CompletedTask;
		}

		static async Task DBCount() {
			while (true) {
				try {
					IMongoCollection<AllowUsers> DBAllowUsersCollection = MongoDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);
					List<AllowUsers> DBAllowUsersAllList = await (await DBAllowUsersCollection.FindAsync(_ => true).ConfigureAwait(false)).ToListAsync().ConfigureAwait(false);

					DiscordGame ReadyStatus = new DiscordGame() {
						Name = CommandConfig.PublicPrefix + "help" + " | " + DBAllowUsersAllList.Count + " " + "Users!",
					};
					await Client.Bot.UpdateStatusAsync(ReadyStatus, UserStatus.Online).ConfigureAwait(false);
				}
				catch (InvalidOperationException) {
					DiscordGame ReadyStatus = new DiscordGame() {
						Name = CommandConfig.PublicPrefix + "help",
					};
					await Client.Bot.UpdateStatusAsync(ReadyStatus, UserStatus.Online).ConfigureAwait(false);
				}

				await Task.Delay(1000).ConfigureAwait(false);
			}
		}
	}
}
