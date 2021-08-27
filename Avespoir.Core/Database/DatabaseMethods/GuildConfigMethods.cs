using Avespoir.Core.Database.Schemas;
using LinqToDB;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class GuildConfigMethods {

		internal static ITable<GuildConfig> GuildConfigTable =>
			MySqlClient.Database.GetTable<GuildConfig>();

		internal static GuildConfig FindOne(Func<GuildConfig, bool> WhereFunc) =>
			GuildConfigTable.Where(WhereFunc).FirstOrDefault();

		internal static bool WhitelistFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.WhiteList ?? false;

		internal static bool LeaveBanFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LeaveBan ?? false;

		internal static string PrefixFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.Prefix;

		internal static string PublicPrefixFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.PublicPrefix;

		internal static string ModeratorPrefixFind(ulong GuildID) =>
			FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.ModeratorPrefix;

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
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.WhiteList, AfterWhitelist)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					WhiteList = AfterWhitelist
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.WhiteList, InsertGuildConfig.WhiteList)
				.Insert();
			}
		}

		internal static void LeaveBanUpsert(ulong GuildID, bool AfterLeaveBan) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.LeaveBan, AfterLeaveBan)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LeaveBan = AfterLeaveBan
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.LeaveBan, InsertGuildConfig.LeaveBan)
				.Insert();
			}
		}

		internal static void PrefixUpsert(ulong GuildID, string AfterPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.Prefix, AfterPrefix)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Prefix = AfterPrefix
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.Prefix, InsertGuildConfig.Prefix)
				.Insert();
			}
		}

		internal static void PublicPrefixUpsert(ulong GuildID, string AfterPublicPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.PublicPrefix, AfterPublicPrefix)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					PublicPrefix = AfterPublicPrefix
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.PublicPrefix, InsertGuildConfig.PublicPrefix)
				.Insert();
			}
		}

		internal static void ModeratorPrefixUpsert(ulong GuildID, string AfterModeratorPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.ModeratorPrefix, AfterModeratorPrefix)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					ModeratorPrefix = AfterModeratorPrefix
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.ModeratorPrefix, InsertGuildConfig.ModeratorPrefix)
				.Insert();
			}
		}

		internal static void LogChannelIdUpsert(ulong GuildID, ulong AfterLogChannelId) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.LogChannelId, AfterLogChannelId)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LogChannelId = AfterLogChannelId
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.LogChannelId, InsertGuildConfig.LogChannelId)
				.Insert();
			}
		}

		internal static void LanguageUpsert(ulong GuildID, Enums.Language AfterLanguage) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.Language, Enum.GetName(typeof(Enums.Language), AfterLanguage))
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Language = Enum.GetName(typeof(Enums.Language), AfterLanguage)
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.Language, InsertGuildConfig.Language)
				.Insert();
			}
		}

		internal static void LevelSwitchUpsert(ulong GuildID, bool AfterLevelSwitch) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				GuildConfigTable
				.Where(Guild_Config => Guild_Config.Id == DBGuildConfig.Id)
				.Set(Guild_Config => Guild_Config.LevelSwitch, AfterLevelSwitch)
				.Update();
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LevelSwitch = AfterLevelSwitch
				};

				GuildConfigTable
				.Value(x => x.GuildID, InsertGuildConfig.GuildID)
				.Value(x => x.LevelSwitch, InsertGuildConfig.LevelSwitch)
				.Insert();
			}
		}
	}
}
