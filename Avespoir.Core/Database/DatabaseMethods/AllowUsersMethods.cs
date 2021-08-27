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

		internal static AllowUsers FindOne(Func<AllowUsers, bool> WhereFunc) =>
			AllowUsersTable.Where(WhereFunc).FirstOrDefault();

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
			DBAllowUsers = (
				from AllowUser in AllowUsersTable
				where AllowUser.GuildID == GuildID
				select AllowUser
			).ToList();

			return DBAllowUsers != null;
		}

		internal static bool AllowUserUpdate(ulong GuildID, ulong Uuid, string Name = null, uint? RoleNum = null) {
			if (AllowUserFind(GuildID, Uuid, out AllowUsers DBAllowUser)) {
				Name ??= DBAllowUser.Name;
				RoleNum ??= DBAllowUser.RoleNum;

				AllowUsersTable
				.Where(AllowUser => AllowUser.Id == DBAllowUser.Id)
				.Set(AllowUser => AllowUser.Name, Name)
				.Set(AllowUser => AllowUser.RoleNum, (uint) RoleNum)
				.Update();

				return true;
			}
			else return false;
		}

		internal static bool AllowUserUpdate(AllowUsers DBAllowUser) {
			try {
				AllowUsersTable
				.Where(AllowUser => AllowUser.Id == DBAllowUser.Id)
				.Set(x => x, DBAllowUser)
				.Update();
				return true;
			}
			catch (Exception) {
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

			AllowUsersTable
			.Value(x => x.GuildID, InsertAllowUser.GuildID)
			.Value(x => x.Uuid, InsertAllowUser.Uuid)
			.Value(x => x.Name, InsertAllowUser.Name)
			.Value(x => x.RoleNum, InsertAllowUser.RoleNum)
			.Insert();

			return InsertAllowUser;
		}

		internal static bool AllowUserDelete(AllowUsers AllowUser) {
			try {
				AllowUsersTable.Where(Allow_User => Allow_User.Id == AllowUser.Id).Delete();
				return true;
			}
			catch (Exception) {
				return false;
			}
		}
	}
}
