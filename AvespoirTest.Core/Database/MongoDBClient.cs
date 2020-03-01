using AvespoirTest.Core.Configs;
using AvespoirTest.Core.Database.Schemas;
using AvespoirTest.Core.Modules.Logger;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvespoirTest.Core.Database {

	class MongoDBClient {

		MongoClientSettings MongoConfig = new MongoDBConfigs().ClientSettings;

		internal static MongoClient Client;

		internal static IMongoDatabase Database;

		internal MongoDBClient() {
			ConnectDB();
		}

		void ConnectDB(int TimeoutCount = 0) {
			try {
				new InfoLog("Connecting to database...");
				Client = new MongoClient(MongoConfig);
				Database = Client.GetDatabase("TestDiscordBot");
				IMongoCollection<AllowUsers> Collection = Database.GetCollection<AllowUsers>("AllowUsers");
				List<AllowUsers> users = Collection.Find(new BsonDocument()).ToList();
				foreach (AllowUsers x in users) {
					Console.WriteLine(x.id);
				}
			}
			catch (TimeoutException Error) {
				new ErrorLog(Error);

				// Max 1 Day
				int Second = TimeoutCount > 17280 - 1 ? 17280 * 5 : (TimeoutCount + 1) * 5;
				new InfoLog($"Reconnect after {Second} seconds");
				Task.Delay(Second * 1000).Wait();
				ConnectDB(TimeoutCount + 1);
			}
		}
	}
}
