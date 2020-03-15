using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Modules.Logger;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Database {

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
				new InfoLog("Connecting to database...");

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

						new InfoLog("Connected to database!");

						CheckPing = true;
					}
					else continue;
				}
			}
			catch (TimeoutException Error) {
				new ErrorLog(Error);

				// Max 1 Day
				int Second = TimeoutCount > 17280 - 1 ? 17280 * 5 : (TimeoutCount + 1) * 5;
				new InfoLog($"Reconnect after {Second} seconds");

				Task.Delay(Second * 1000).ConfigureAwait(false).GetAwaiter().GetResult();
				await Main(TimeoutCount + 1);
			}
		}
	}
}
