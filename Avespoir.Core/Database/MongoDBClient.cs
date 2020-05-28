﻿using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Logger;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Database {

	class MongoDBClient {

		static MongoClientSettings MongoConfig = new MongoDBConfigs().ClientSettings;

		internal static string MainDatabase { get; set; } = "DiscordBot";

		internal static MongoClient Client { get; private set; }

		internal static IMongoDatabase Database { get; private set; }

		internal static void DeleteDBAccess() {
			Client = default;
			Database = default;
		}

		internal static async Task Main(int TimeoutCount = 0) {
			try {
				Log.Info("Connecting to database...");

				MongoClient PingTest = new MongoClient(MongoConfig);

				bool CheckPing = false;
				while (!CheckPing) {
					Task<BsonDocument> PingTask = PingTest.GetDatabase(MainDatabase).RunCommandAsync((Command<BsonDocument>)"{ ping: 1 }");
					
					bool Ping = false;
					if (!PingTask.IsCompleted) {
						await PingTask;
						Ping = true;
					}

					if (Ping) {
						Client = new MongoClient(MongoConfig);
						Database = Client.GetDatabase(MainDatabase);

						Log.Info("Connected to database!");

						CheckPing = true;
					}
					else continue;
				}
			}
			catch (TimeoutException Error) {
				Log.Error(Error);

				// Max 1 Day
				int Second = TimeoutCount > 17280 - 1 ? 17280 * 5 : (TimeoutCount + 1) * 5;
				Log.Info($"Reconnect after {Second} seconds");

				Task.Delay(Second * 1000).ConfigureAwait(false).GetAwaiter().GetResult();
				await Main(TimeoutCount + 1);
			}
		}
	}
}
