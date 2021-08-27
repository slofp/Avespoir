using Avespoir.Core.Database.Schemas;
using LinqToDB;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class UserDataMethods {

		internal static ITable<UserData> UserDataTable =>
			MySqlClient.Database.GetTable<UserData>();

		internal static UserData FindOne(Func<UserData, bool> WhereFunc) =>
			UserDataTable.Where(WhereFunc).FirstOrDefault();

		internal static double ExpFind(ulong UserID) => 
			FindOne(User_Data => User_Data.UserID == UserID)?.ExperiencePoint ?? 0;

		internal static uint LevelFind(ulong UserID) =>
			FindOne(User_Data => User_Data.UserID == UserID)?.Level ?? 1;

		private static bool UserDataFind(ulong UserID, [MaybeNullWhen(true)] out UserData DBUserData) {
			DBUserData = FindOne(User_Data => User_Data.UserID == UserID);

			return DBUserData != null;
		}

		internal static void DataUpsert(ulong UserID, uint Level, double Exp) {
			if (UserDataFind(UserID, out UserData DBUserData)) {
				UserDataTable
				.Where(User_Data => User_Data.Id == DBUserData.Id)
				.Set(User_Data => User_Data.ExperiencePoint, Exp)
				.Set(User_Data => User_Data.Level, Level)
				.Update();
			}
			else {
				UserData InsertUserData = new UserData {
					UserID = UserID,
					ExperiencePoint = Exp,
					Level = Level
				};

				UserDataTable.Value(x => x, InsertUserData).Insert();
			}
		}
	}
}
