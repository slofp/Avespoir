using Avespoir.Core.Database.Schemas;
using LinqToDB;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class UserDataMethods {

		internal static ITable<UserData> UserDataTable =>
			MySqlClient.Database.GetTable<UserData>();

		internal static UserData FindOne(Func<UserData, bool> WhereFunc) {
			try {
				MySqlClient.Database.BeginTransaction();
				UserData Result = UserDataTable.Where(WhereFunc).FirstOrDefault();
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
				try {
					MySqlClient.Database.BeginTransaction();
					UserDataTable
					.Where(User_Data => User_Data.Id == DBUserData.Id)
					.Set(User_Data => User_Data.ExperiencePoint, Exp)
					.Set(User_Data => User_Data.Level, Level)
					.Update();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					DataUpsert(UserID, Level, Exp);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
			else {
				UserData InsertUserData = new UserData {
					UserID = UserID,
					ExperiencePoint = Exp,
					Level = Level
				};

				try {
					MySqlClient.Database.BeginTransaction();
					UserDataTable
					.Value(x => x.UserID, InsertUserData.UserID)
					.Value(x => x.ExperiencePoint, InsertUserData.ExperiencePoint)
					.Value(x => x.Level, InsertUserData.Level)
					.Insert();
					MySqlClient.Database.CommitTransaction();
				}
				catch (MySql.Data.MySqlClient.MySqlException) {
					MySqlClient.DBUpdate();

					DataUpsert(UserID, Level, Exp);
				}
				catch (Exception) {
					MySqlClient.Database.RollbackTransaction();

					throw;
				}
			}
		}
	}
}
