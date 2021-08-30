using Avespoir.Core.Database.Schemas;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class AllowUsersMethods {

		internal static ITable<AllowUsers> AllowUsersTable =>
			MySqlClient.Database.GetTable<AllowUsers>();

		internal static AllowUsers FindOne(Func<AllowUsers, bool> WhereFunc) {
			try {
				MySqlClient.Database.BeginTransaction();
				AllowUsers Result = AllowUsersTable.Where(WhereFunc).FirstOrDefault();
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

		internal static bool AllowUserExist(ulong GuildID, ulong Uuid) => AllowUserFind(GuildID, Uuid, out AllowUsers _);

		internal static bool AllowUserExist(ulong GuildID, string Name) => AllowUserFind(GuildID, Name, out AllowUsers _);

		internal static bool AllowUserFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(true)] out AllowUsers DBAllowUser) {
			DBAllowUser = FindOne(AllowUser => AllowUser.GuildID == GuildID & AllowUser.Uuid == Uuid);

			return DBAllowUser != null;
		}

		internal static bool AllowUserFind(ulong GuildID, string Name, [MaybeNullWhen(true)] out AllowUsers DBAllowUser) {
			DBAllowUser = FindOne(AllowUser => AllowUser.GuildID == GuildID & AllowUser.Name == Name);

			return DBAllowUser != null;
		}

		internal static bool AllowUsersListFind(ulong GuildID, [MaybeNullWhen(true)] out List<AllowUsers> DBAllowUsers) {
			try {
				MySqlClient.Database.BeginTransaction();

				DBAllowUsers = (
					from AllowUser in AllowUsersTable
					where AllowUser.GuildID == GuildID
					select AllowUser
				).ToList();

				MySqlClient.Database.CommitTransaction();

				return DBAllowUsers != null;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return AllowUsersListFind(GuildID, out DBAllowUsers);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				throw;
			}
		}

		internal static bool AllowUserUpdate(ulong GuildID, ulong Uuid, string Name = null, uint? RoleNum = null) {
			try {
				if (AllowUserFind(GuildID, Uuid, out AllowUsers DBAllowUser)) {
					Name ??= DBAllowUser.Name;
					RoleNum ??= DBAllowUser.RoleNum;

					MySqlClient.Database.BeginTransaction();
					AllowUsersTable
					.Where(AllowUser => AllowUser.Id == DBAllowUser.Id)
					.Set(AllowUser => AllowUser.Name, Name)
					.Set(AllowUser => AllowUser.RoleNum, (uint) RoleNum)
					.Update();

					MySqlClient.Database.CommitTransaction();

					return true;
				}
				else return false;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return AllowUserUpdate(GuildID, Uuid, Name, RoleNum);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				return false;
			}
		}

		internal static bool AllowUserUpdate(AllowUsers DBAllowUser) {
			try {
				MySqlClient.Database.BeginTransaction();

				AllowUsersTable
				.Where(AllowUser => AllowUser.Id == DBAllowUser.Id)
				.Set(x => x, DBAllowUser)
				.Update();

				MySqlClient.Database.CommitTransaction();

				return true;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return AllowUserUpdate(DBAllowUser);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				return false;
			}
		}

		internal static AllowUsers AllowUserInsert(ulong GuildID, ulong Uuid, string Name, uint RoleNum) {
			AllowUsers InsertAllowUser = new AllowUsers {
				GuildID = GuildID,
				Uuid = Uuid,
				Name = Name,
				RoleNum = RoleNum
			};

			try {
				MySqlClient.Database.BeginTransaction();
				AllowUsersTable
				.Value(x => x.GuildID, InsertAllowUser.GuildID)
				.Value(x => x.Uuid, InsertAllowUser.Uuid)
				.Value(x => x.Name, InsertAllowUser.Name)
				.Value(x => x.RoleNum, InsertAllowUser.RoleNum)
				.Insert();

				MySqlClient.Database.CommitTransaction();
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return AllowUserInsert(GuildID, Uuid, Name, RoleNum);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				throw;
			}

			return InsertAllowUser;
		}

		internal static bool AllowUserDelete(AllowUsers AllowUser) {
			try {
				MySqlClient.Database.BeginTransaction();
				AllowUsersTable.Where(Allow_User => Allow_User.Id == AllowUser.Id).Delete();
				MySqlClient.Database.CommitTransaction();
				return true;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return AllowUserDelete(AllowUser);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();
				return false;
			}
		}
	}
}
