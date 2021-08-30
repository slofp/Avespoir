using Avespoir.Core.Configs;
using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;

namespace Avespoir.Core.Database {

	class MySqlClient {

		internal static DataConnection Database { get; set; }

		private static void CreateIfNotExist<T>() {
			Database.CommitTransaction();
			try {
				Database.BeginTransaction();
				Database.CreateTable<T>(tableOptions: TableOptions.CreateIfNotExists);
				Database.CommitTransaction();
			}
			catch (Exception Error) when (Error is NotSupportedException | Error is KeyNotFoundException) {
				Log.Warning("CreateTable error. process will continue.");
				// System.NotSupportedException: 'Character set 'utf8mb3' is not supported by .Net Framework.'
				// System.Collections.Generic.KeyNotFoundException: 'The given key '29541' was not present in the dictionary.'
				Database.CommitTransaction();
			}
			catch (Exception Error) {
				Database.RollbackTransaction();
				throw Error;
			}
		}

		private static void Init() {
			Log.Info("Initialize database...");

			CreateIfNotExist<AllowUsers>();
			CreateIfNotExist<GuildConfig>();
			CreateIfNotExist<Roles>();
			CreateIfNotExist<UserData>();

			Log.Info("Initialized!");
		}

		internal static void Main() {
			Log.Info("Connecting to database...");

			Database = new DataConnection(new LinqToDB.Configuration.LinqToDbConnectionOptionsBuilder().UseConnectionString(ProviderName.MySql, MySqlConfigs.ConnectionString).Build());
			Database.OnConnectionOpened += Database_OnConnectionOpened;
			Database.OnClosed += Database_OnClosed;
			Database.OnClosing += Database_OnClosing;

			Log.Info("Connected to database!");

			Init();
		}

		private static void Database_OnClosing(object sender, EventArgs e) {
			Log.Info("Database connection closing...");
		}

		private static void Database_OnClosed(object sender, EventArgs e) {
			Log.Info("Database connection closed.");
		}

		private static void Database_OnConnectionOpened(DataConnection arg1, System.Data.IDbConnection arg2) {
			Log.Info("Database connection opened.");
		}

		internal static void DBUpdate() {
			DeleteDBAccess();
			Main();
		}

		internal static void DeleteDBAccess() {
			Database.CommitTransaction();
			Log.Info("Database Commited.");
			Database.Close();
			Log.Info("Database closed.");
			Database.Dispose();
			Log.Info("Database Disposed.");
			Database = default;
			Log.Info("Database access removed.");
		}
	}
}
