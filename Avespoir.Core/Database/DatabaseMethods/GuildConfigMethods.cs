using Avespoir.Core.Database.Schemas;
using LinqToDB;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class GuildConfigMethods {

		internal static ITable<GuildConfig> GuildConfigTable =>
			MySqlClient.Database.GetTable<GuildConfig>();

		internal static GuildConfig FindOne(Func<GuildConfig, bool> WhereFunc) {
			try {
				MySqlClient.Database.BeginTransaction();
				GuildConfig Result = GuildConfigTable.Where(WhereFunc).FirstOrDefault();
				MySqlClient.Database.CommitTransaction();

				return Result;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return FindOne(WhereFunc);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				throw;
			}
		}

		internal static bool WhitelistFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.WhiteList ?? false;

		internal static bool LeaveBanFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LeaveBan ?? false;

		internal static string PrefixFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.Prefix;

		internal static ulong LogChannelFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LogChannelId ?? 0;

		internal static string LanguageFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.Language;

		internal static bool LevelSwitchFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LevelSwitch ?? true;

		private static bool GuildConfigFind(ulong GuildID, [MaybeNullWhen(true)] out GuildConfig DBGuildConfig) {
			DBGuildConfig = FindOne(Guild_Config => Guild_Config.GuildID == GuildID);

			return DBGuildConfig != null;
		}

		internal static void WhitelistUpsert(ulong GuildID, bool AfterWhitelist) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
					.Set(Guild_Config => Guild_Config.WhiteList, AfterWhitelist)
					.Update();

					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					WhitelistUpsert(GuildID, AfterWhitelist);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					WhiteList = AfterWhitelist
				};

				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Value(x => x.GuildID, InsertGuildConfig.GuildID)
					.Value(x => x.WhiteList, InsertGuildConfig.WhiteList)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					WhitelistUpsert(GuildID, AfterWhitelist);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}

		internal static void LeaveBanUpsert(ulong GuildID, bool AfterLeaveBan) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
					.Set(Guild_Config => Guild_Config.LeaveBan, AfterLeaveBan)
					.Update();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LeaveBanUpsert(GuildID, AfterLeaveBan);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LeaveBan = AfterLeaveBan
				};

				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Value(x => x.GuildID, InsertGuildConfig.GuildID)
					.Value(x => x.LeaveBan, InsertGuildConfig.LeaveBan)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LeaveBanUpsert(GuildID, AfterLeaveBan);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}

		internal static void PrefixUpsert(ulong GuildID, string AfterPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
					.Set(Guild_Config => Guild_Config.Prefix, AfterPrefix)
					.Update();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					PrefixUpsert(GuildID, AfterPrefix);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Prefix = AfterPrefix
				};

				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Value(x => x.GuildID, InsertGuildConfig.GuildID)
					.Value(x => x.Prefix, InsertGuildConfig.Prefix)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					PrefixUpsert(GuildID, AfterPrefix);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}

		internal static void LogChannelIdUpsert(ulong GuildID, ulong AfterLogChannelId) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
					.Set(Guild_Config => Guild_Config.LogChannelId, AfterLogChannelId)
					.Update();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LogChannelIdUpsert(GuildID, AfterLogChannelId);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LogChannelId = AfterLogChannelId
				};

				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Value(x => x.GuildID, InsertGuildConfig.GuildID)
					.Value(x => x.LogChannelId, InsertGuildConfig.LogChannelId)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LogChannelIdUpsert(GuildID, AfterLogChannelId);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}

		internal static void LanguageUpsert(ulong GuildID, Enums.Language AfterLanguage) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
					.Set(Guild_Config => Guild_Config.Language, Enum.GetName(typeof(Enums.Language), AfterLanguage))
					.Update();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LanguageUpsert(GuildID, AfterLanguage);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Language = Enum.GetName(typeof(Enums.Language), AfterLanguage)
				};

				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Value(x => x.GuildID, InsertGuildConfig.GuildID)
					.Value(x => x.Language, InsertGuildConfig.Language)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LanguageUpsert(GuildID, AfterLanguage);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}

		internal static void LevelSwitchUpsert(ulong GuildID, bool AfterLevelSwitch) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
					.Set(Guild_Config => Guild_Config.LevelSwitch, AfterLevelSwitch)
					.Update();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LevelSwitchUpsert(GuildID, AfterLevelSwitch);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LevelSwitch = AfterLevelSwitch
				};

				try {
					MySqlClient.Database.BeginTransaction();
					GuildConfigTable
					.Value(x => x.GuildID, InsertGuildConfig.GuildID)
					.Value(x => x.LevelSwitch, InsertGuildConfig.LevelSwitch)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					LevelSwitchUpsert(GuildID, AfterLevelSwitch);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}
	}
}
