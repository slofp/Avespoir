using Avespoir.Core.Database.Schemas;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class UserDataMethods {

		private static UserData UserDataFind(ulong UserID) => (
			from User_Data in MongoDBClient.UserDataCollection.AsQueryable()
			where User_Data.UserID == UserID
			select User_Data
		).FirstOrDefault();

		internal static double ExpFind(ulong UserID) =>
			UserDataFind(UserID)?.ExperiencePoint ?? 0;

		internal static uint LevelFind(ulong UserID) =>
			UserDataFind(UserID)?.Level ?? 1;

		private static bool UserDataFind(ulong UserID, [MaybeNullWhen(false)] out UserData DBUserData) {
			DBUserData = UserDataFind(UserID);

			return DBUserData != null;
		}

		internal static void DataUpsert(ulong UserID, uint Level, double Exp) {
			if (UserDataFind(UserID, out UserData DBUserData)) {
				UpdateDefinition<UserData> UpdateDef = Builders<UserData>.Update
					.Set(User_Data => User_Data.ExperiencePoint, Exp)
					.Set(User_Data => User_Data.Level, Level);
				MongoDBClient.UserDataCollection.UpdateOne(User_Data => User_Data.Id == DBUserData.Id, UpdateDef);
			}
			else {
				UserData InsertUserData = new UserData {
					UserID = UserID,
					ExperiencePoint = Exp,
					Level = Level
				};

				MongoDBClient.UserDataCollection.InsertOne(InsertUserData);
			}
		}
	}
}
