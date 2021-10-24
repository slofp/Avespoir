using Avespoir.Core.Configs;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Avespoir.Core.Database {

	class MongoDBClient {

		private static MongoClient Connection { get; set; }

		private static IMongoDatabase Database => Connection.GetDatabase(MongoDBConfigs.Database);

		internal static IMongoCollection<AllowUsers> AllowUsersCollection =>
			Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);

		internal static IMongoCollection<GuildConfig> GuildConfigCollection =>
			Database.GetCollection<GuildConfig>(typeof(GuildConfig).Name);

		internal static IMongoCollection<Roles> RolesCollection =>
			Database.GetCollection<Roles>(typeof(Roles).Name);

		internal static IMongoCollection<UserData> UserDataCollection =>
			Database.GetCollection<UserData>(typeof(UserData).Name);

		internal static void Main() {
			Log.Info("Connecting to database...");

			if (Connection != null) {
				Log.Error("Database is already exist");
				return;
			}

			Connection = new MongoClient(MongoDBConfigs.ClientSettings);

			Log.Info("Connected to database!");
		}

		internal static async Task DropUserData() {
			await Database.DropCollectionAsync(typeof(UserData).Name);
		}

		internal static void DeleteDBAccess() {
			Connection = default;
			Log.Info("Database access removed.");
		}
	}
}
