using Avespoir.Core.Database.Schemas;
using LiteDB;
using System.Diagnostics.CodeAnalysis;

namespace Avespoir.Core.Database.DatabaseMethods {

	class UserDataMethods {

		internal static ILiteCollection<UserData> UserDataCollection =>
			LiteDBClient.Database.GetCollection<UserData>(typeof(UserData).Name);

		internal static double ExpFind(ulong UserID) =>
			UserDataCollection.FindOne(User_Data => User_Data.UserID == UserID)?.ExperiencePoint ?? 0;

		internal static uint LevelFind(ulong UserID) =>
			UserDataCollection.FindOne(User_Data => User_Data.UserID == UserID)?.Level ?? 1;

		private static bool UserDataFind(ulong UserID, [MaybeNullWhen(true)] out UserData DBUserData) {
			DBUserData = UserDataCollection.FindOne(User_Data => User_Data.UserID == UserID);

			return DBUserData != null;
		}

		internal static void DataUpsert(ulong UserID, uint Level, double Exp) {
			if (UserDataFind(UserID, out UserData DBUserData)) {
				DBUserData.ExperiencePoint = Exp;
				DBUserData.Level = Level;

				UserDataCollection.Update(DBUserData);
			}
			else {
				UserData InsertUserData = new UserData {
					UserID = UserID,
					ExperiencePoint = Exp,
					Level = Level
				};

				UserDataCollection.Insert(InsertUserData);
			}
		}
	}
}
