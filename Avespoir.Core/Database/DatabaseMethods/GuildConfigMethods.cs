using Avespoir.Core.Database.Schemas;
using LiteDB;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Avespoir.Core.Database.DatabaseMethods {

	class GuildConfigMethods {

		internal static ILiteCollection<GuildConfig> GuildConfigCollection =>
			LiteDBClient.Database.GetCollection<GuildConfig>(typeof(GuildConfig).Name);

		internal static bool WhitelistFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.WhiteList ?? false;

		internal static bool LeaveBanFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LeaveBan ?? false;

		internal static string PublicPrefixFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.PublicPrefix;

		internal static string ModeratorPrefixFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.ModeratorPrefix;

		internal static ulong LogChannelFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LogChannelId ?? 0;

		internal static string LanguageFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.Language;

		internal static bool LevelSwitchFind(ulong GuildID) =>
			GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID)?.LevelSwitch ?? true;

		private static bool GuildConfigFind(ulong GuildID, [MaybeNullWhen(true)] out GuildConfig DBGuildConfig) {
			DBGuildConfig = GuildConfigCollection.FindOne(Guild_Config => Guild_Config.GuildID == GuildID);

			return DBGuildConfig != null;
		}

		internal static void WhitelistUpsert(ulong GuildID, bool AfterWhitelist) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.WhiteList = AfterWhitelist;

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					WhiteList = AfterWhitelist
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}

		internal static void LeaveBanUpsert(ulong GuildID, bool AfterLeaveBan) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.LeaveBan = AfterLeaveBan;

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LeaveBan = AfterLeaveBan
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}

		internal static void PublicPrefixUpsert(ulong GuildID, string AfterPublicPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.PublicPrefix = AfterPublicPrefix;

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					PublicPrefix = AfterPublicPrefix
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}

		internal static void ModeratorPrefixUpsert(ulong GuildID, string AfterModeratorPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.ModeratorPrefix = AfterModeratorPrefix;

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					ModeratorPrefix = AfterModeratorPrefix
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}

		internal static void LogChannelIdUpsert(ulong GuildID, ulong AfterLogChannelId) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.LogChannelId = AfterLogChannelId;

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LogChannelId = AfterLogChannelId
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}

		internal static void LanguageUpsert(ulong GuildID, Enums.Language AfterLanguage) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.Language = Enum.GetName(typeof(Enums.Language), AfterLanguage);

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Language = Enum.GetName(typeof(Enums.Language), AfterLanguage)
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}

		internal static void LevelSwitchUpsert(ulong GuildID, bool AfterLevelSwitch) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				DBGuildConfig.LevelSwitch = AfterLevelSwitch;

				GuildConfigCollection.Update(DBGuildConfig);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LevelSwitch = AfterLevelSwitch
				};

				GuildConfigCollection.Insert(InsertGuildConfig);
			}
		}
	}
}
