using Avespoir.Core.Database.Schemas;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Avespoir.Core.Database {

	class DatabaseMethods {
		#region GuildConfig

		static readonly IMongoCollection<GuildConfig> DBGuildConfigCollection = MongoDBClient.Database.GetCollection<GuildConfig>(typeof(GuildConfig).Name);

		internal static async Task<bool> WhitelistFind(ulong GuildID) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBGuildConfig.WhiteList;
			}
			catch (InvalidOperationException) {
				return false;
			}
		}

		internal static async Task<bool> LeaveBanFind(ulong GuildID) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBGuildConfig.LeaveBan;
			}
			catch (InvalidOperationException) {
				return false;
			}
		}

		internal static async Task<string> PublicPrefixFind(ulong GuildID) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBGuildConfig.PublicPrefix;
			}
			catch (InvalidOperationException) {
				return null;
			}
		}

		internal static async Task<string> ModeratorPrefixFind(ulong GuildID) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBGuildConfig.ModeratorPrefix;
			}
			catch (InvalidOperationException) {
				return null;
			}
		}

		internal static async Task<ulong> LogChannelFind(ulong GuildID) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBGuildConfig.LogChannelId;
			}
			catch (InvalidOperationException) {
				return 0;
			}
		}

		internal static async Task<string> LanguageFind(ulong GuildID) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBGuildConfig.Language;
			}
			catch (InvalidOperationException) {
				return null;
			}
		}

		internal static async Task WhitelistUpsert(ulong GuildID, bool AfterWhitelist) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<GuildConfig> UpdateWhitelist = Builders<GuildConfig>.Update.Set(Guild_Config => Guild_Config.WhiteList, AfterWhitelist);
				await DBGuildConfigCollection.UpdateOneAsync(DBGuildConfigFilter, UpdateWhitelist).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					WhiteList = AfterWhitelist
				};

				await DBGuildConfigCollection.InsertOneAsync(InsertGuildConfig).ConfigureAwait(false);
			}
		}

		internal static async Task LeaveBanUpsert(ulong GuildID, bool AfterLeaveBan) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<GuildConfig> UpdateLeaveBan = Builders<GuildConfig>.Update.Set(Guild_Config => Guild_Config.LeaveBan, AfterLeaveBan);
				await DBGuildConfigCollection.UpdateOneAsync(DBGuildConfigFilter, UpdateLeaveBan).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LeaveBan = AfterLeaveBan
				};

				await DBGuildConfigCollection.InsertOneAsync(InsertGuildConfig).ConfigureAwait(false);
			}
		}

		internal static async Task PublicPrefixUpsert(ulong GuildID, string AfterPublicPrefix) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<GuildConfig> UpdatePublicPrefix = Builders<GuildConfig>.Update.Set(Guild_Config => Guild_Config.PublicPrefix, AfterPublicPrefix);
				await DBGuildConfigCollection.UpdateOneAsync(DBGuildConfigFilter, UpdatePublicPrefix).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					PublicPrefix = AfterPublicPrefix
				};

				await DBGuildConfigCollection.InsertOneAsync(InsertGuildConfig).ConfigureAwait(false);
			}
		}

		internal static async Task ModeratorPrefixUpsert(ulong GuildID, string AfterModeratorPrefix) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<GuildConfig> UpdateModeratorPrefix = Builders<GuildConfig>.Update.Set(Guild_Config => Guild_Config.ModeratorPrefix, AfterModeratorPrefix);
				await DBGuildConfigCollection.UpdateOneAsync(DBGuildConfigFilter, UpdateModeratorPrefix).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					ModeratorPrefix = AfterModeratorPrefix
				};

				await DBGuildConfigCollection.InsertOneAsync(InsertGuildConfig).ConfigureAwait(false);
			}
		}

		internal static async Task LogChannelIdUpsert(ulong GuildID, ulong AfterLogChannelId) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<GuildConfig> UpdateLogChannelId = Builders<GuildConfig>.Update.Set(Guild_Config => Guild_Config.LogChannelId, AfterLogChannelId);
				await DBGuildConfigCollection.UpdateOneAsync(DBGuildConfigFilter, UpdateLogChannelId).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					LogChannelId = AfterLogChannelId
				};

				await DBGuildConfigCollection.InsertOneAsync(InsertGuildConfig).ConfigureAwait(false);
			}
		}

		internal static async Task LanguageUpsert(ulong GuildID, Enums.Language AfterLanguage) {
			try {
				FilterDefinition<GuildConfig> DBGuildConfigFilter = Builders<GuildConfig>.Filter.Eq(Guild_Config => Guild_Config.GuildID, GuildID);
				GuildConfig DBGuildConfig = await (await DBGuildConfigCollection.FindAsync(DBGuildConfigFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<GuildConfig> UpdateLanguage = Builders<GuildConfig>.Update.Set(Guild_Config => Guild_Config.Language, Enum.GetName(typeof(Enums.Language), AfterLanguage));
				await DBGuildConfigCollection.UpdateOneAsync(DBGuildConfigFilter, UpdateLanguage).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				GuildConfig InsertGuildConfig = new GuildConfig {
					GuildID = GuildID,
					Language = Enum.GetName(typeof(Enums.Language), AfterLanguage)
				};

				await DBGuildConfigCollection.InsertOneAsync(InsertGuildConfig).ConfigureAwait(false);
			}
		}
		#endregion

		#region UserData

		static readonly IMongoCollection<UserData> DBUserDataCollection = MongoDBClient.Database.GetCollection<UserData>(typeof(UserData).Name);

		internal static async Task<double> ExpFind(ulong UserID) {
			try {
				FilterDefinition<UserData> DBUserDataFilter = Builders<UserData>.Filter.Eq(User_Data => User_Data.UserID, UserID);
				UserData DBUserData = await (await DBUserDataCollection.FindAsync(DBUserDataFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBUserData.ExperiencePoint;
			}
			catch (InvalidOperationException) {
				return 0;
			}
		}

		internal static async Task<uint> LevelFind(ulong UserID) {
			try {
				FilterDefinition<UserData> DBUserDataFilter = Builders<UserData>.Filter.Eq(User_Data => User_Data.UserID, UserID);
				UserData DBUserData = await (await DBUserDataCollection.FindAsync(DBUserDataFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBUserData.Level;
			}
			catch (InvalidOperationException) {
				return 1;
			}
		}

		internal static async Task DataUpsert(ulong UserID, uint Level, double Exp) {
			try {
				FilterDefinition<UserData> DBUserDataFilter = Builders<UserData>.Filter.Eq(User_Data => User_Data.UserID, UserID);
				UserData DBUserData = await (await DBUserDataCollection.FindAsync(DBUserDataFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				UpdateDefinition<UserData> UpdateUserExpData = Builders<UserData>.Update.Set(User_Data => User_Data.ExperiencePoint, Exp);
				await DBUserDataCollection.UpdateOneAsync(DBUserDataFilter, UpdateUserExpData).ConfigureAwait(false);

				UpdateDefinition<UserData> UpdateUserLevelData = Builders<UserData>.Update.Set(User_Data => User_Data.Level, Level);
				await DBUserDataCollection.UpdateOneAsync(DBUserDataFilter, UpdateUserLevelData).ConfigureAwait(false);
			}
			catch (InvalidOperationException) {
				UserData InsertUserData = new UserData {
					UserID = UserID,
					ExperiencePoint = Exp,
					Level = Level
				};

				await DBUserDataCollection.InsertOneAsync(InsertUserData).ConfigureAwait(false);
			}
		}
		#endregion
	}
}
