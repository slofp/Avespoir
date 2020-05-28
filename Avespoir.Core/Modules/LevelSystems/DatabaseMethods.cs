using Avespoir.Core.Database;
using Avespoir.Core.Database.Schemas;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Avespoir.Core.Modules.LevelSystems {

	class DatabaseMethods {

		#region LogChannels

		static readonly IMongoCollection<LogChannels> DBLogChannelsCollection = MongoDBClient.Database.GetCollection<LogChannels>(typeof(LogChannels).Name);

		internal static async Task<ulong> LogChannelFind(ulong GuildID) {
			try {
				FilterDefinition<LogChannels> DBLogChannelsFilter = Builders<LogChannels>.Filter.Eq(LogChannel => LogChannel.GuildID, GuildID);
				LogChannels DBLogChannel = await (await DBLogChannelsCollection.FindAsync(DBLogChannelsFilter).ConfigureAwait(false)).FirstAsync().ConfigureAwait(false);

				return DBLogChannel.ChannelID;
			}
			catch (InvalidOperationException) {
				return 0;
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
