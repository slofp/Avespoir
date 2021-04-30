using Avespoir.Core.Configs;
using Avespoir.Core.Modules.Logger;
using LiteDB;

namespace Avespoir.Core.Database {

	class LiteDBClient {

		internal static LiteDatabase Database { get; set; }

		private static readonly ConnectionString LiteDBConfig =
			new ConnectionString {
				Filename = LiteDBConfigs.FileName,
				Password = LiteDBConfigs.Password,
				Connection = ConnectionType.Direct
			};

		internal static void Main() {
			Log.Info("Connecting to database...");

			Database = new LiteDatabase(LiteDBConfig);

			Log.Info("Connected to database!");
		}

		internal static void DeleteDBAccess() {
			if (Database.Commit()) Log.Error("couldn't Database commit.");
			else Log.Info("Database Commited.");
			Database.Dispose();
			Log.Info("Database Disposed.");
			Database = default;
			Log.Info("Database access removed.");
		}
	}
}
