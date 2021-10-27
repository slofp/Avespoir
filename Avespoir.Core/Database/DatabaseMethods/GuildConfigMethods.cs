using Avespoir.Core.Database.Schemas;
using Avespoir.Core.Modules.Logger;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class GuildConfigMethods {

		private static GuildConfig GuildConfigFind(ulong GuildID) => (
			from Guild_Config in MongoDBClient.GuildConfigCollection.AsQueryable()
			where Guild_Config.GuildID == GuildID
			select Guild_Config
		).FirstOrDefault();

		internal static bool WhitelistFind(ulong GuildID) =>
			GuildConfigFind(GuildID)?.WhiteList ?? false;

		internal static bool LeaveBanFind(ulong GuildID) =>
			GuildConfigFind(GuildID)?.LeaveBan ?? false;

		internal static string PrefixFind(ulong GuildID) =>
			GuildConfigFind(GuildID)?.Prefix;

		internal static ulong LogChannelFind(ulong GuildID) =>
			GuildConfigFind(GuildID)?.LogChannelId ?? 0;

		internal static Enums.Language LanguageFind(ulong GuildID) =>
			GuildConfigFind(GuildID)?.Language ?? Enums.Language.ja_JP;

		internal static bool LevelSwitchFind(ulong GuildID) =>
			GuildConfigFind(GuildID)?.LevelSwitch ?? true;

		private static bool GuildConfigFind(ulong GuildID, [MaybeNullWhen(false)] out GuildConfig DBGuildConfig) {
			DBGuildConfig = GuildConfigFind(GuildID);

			return DBGuildConfig != null;
		}

		internal static void WhitelistUpsert(ulong GuildID, bool AfterWhitelist) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				UpdateDefinition<GuildConfig> UpdateDef = Builders<GuildConfig>.Update
					.Set(Guild_Config => Guild_Config.WhiteList, AfterWhitelist);
				MongoDBClient.GuildConfigCollection.UpdateOne(Guild_Config => Guild_Config.Id == DBGuildConfig.Id, UpdateDef);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					WhiteList = AfterWhitelist
				};

				MongoDBClient.GuildConfigCollection.InsertOne(InsertGuildConfig);
			}
		}

		internal static void LeaveBanUpsert(ulong GuildID, bool AfterLeaveBan) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				UpdateDefinition<GuildConfig> UpdateDef = Builders<GuildConfig>.Update
					.Set(Guild_Config => Guild_Config.LeaveBan, AfterLeaveBan);
				MongoDBClient.GuildConfigCollection.UpdateOne(Guild_Config => Guild_Config.Id == DBGuildConfig.Id, UpdateDef);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LeaveBan = AfterLeaveBan
				};

				MongoDBClient.GuildConfigCollection.InsertOne(InsertGuildConfig);
			}
		}

		internal static void PrefixUpsert(ulong GuildID, string AfterPrefix) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				UpdateDefinition<GuildConfig> UpdateDef = Builders<GuildConfig>.Update
					.Set(Guild_Config => Guild_Config.Prefix, AfterPrefix);
				MongoDBClient.GuildConfigCollection.UpdateOne(Guild_Config => Guild_Config.Id == DBGuildConfig.Id, UpdateDef);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Prefix = AfterPrefix
				};

				MongoDBClient.GuildConfigCollection.InsertOne(InsertGuildConfig);
			}
		}

		internal static void LogChannelIdUpsert(ulong GuildID, ulong AfterLogChannelId) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				UpdateDefinition<GuildConfig> UpdateDef = Builders<GuildConfig>.Update
					.Set(Guild_Config => Guild_Config.LogChannelId, AfterLogChannelId);
				MongoDBClient.GuildConfigCollection.UpdateOne(Guild_Config => Guild_Config.Id == DBGuildConfig.Id, UpdateDef);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LogChannelId = AfterLogChannelId
				};

				MongoDBClient.GuildConfigCollection.InsertOne(InsertGuildConfig);
			}
		}

		internal static void LanguageUpsert(ulong GuildID, Enums.Language AfterLanguage) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				UpdateDefinition<GuildConfig> UpdateDef = Builders<GuildConfig>.Update
					.Set(Guild_Config => Guild_Config.Language, AfterLanguage);
				MongoDBClient.GuildConfigCollection.UpdateOne(Guild_Config => Guild_Config.Id == DBGuildConfig.Id, UpdateDef);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Language = AfterLanguage
				};

				MongoDBClient.GuildConfigCollection.InsertOne(InsertGuildConfig);
			}
		}

		internal static void LevelSwitchUpsert(ulong GuildID, bool AfterLevelSwitch) {
			if (GuildConfigFind(GuildID, out GuildConfig DBGuildConfig)) {
				UpdateDefinition<GuildConfig> UpdateDef = Builders<GuildConfig>.Update
					.Set(Guild_Config => Guild_Config.LevelSwitch, AfterLevelSwitch);
				MongoDBClient.GuildConfigCollection.UpdateOne(Guild_Config => Guild_Config.Id == DBGuildConfig.Id, UpdateDef);
			}
			else {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LevelSwitch = AfterLevelSwitch
				};

				MongoDBClient.GuildConfigCollection.InsertOne(InsertGuildConfig);
			}
		}
	}
}
