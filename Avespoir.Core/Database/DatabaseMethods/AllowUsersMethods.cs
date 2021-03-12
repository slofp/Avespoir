using Avespoir.Core.Database.Schemas;
using LiteDB;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class AllowUsersMethods {

		internal static ILiteCollection<AllowUsers> AllowUsersCollection =>
			LiteDBClient.Database.GetCollection<AllowUsers>(typeof(AllowUsers).Name);

		internal static bool AllowUserExist(ulong GuildID, ulong Uuid) => AllowUserFind(GuildID, Uuid, out AllowUsers _);

		internal static bool AllowUserExist(ulong GuildID, string Name) => AllowUserFind(GuildID, Name, out AllowUsers _);

		internal static bool AllowUserFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(true)] out AllowUsers DBAllowUser) {
			DBAllowUser = AllowUsersCollection.FindOne(AllowUser => AllowUser.GuildID == GuildID & AllowUser.Uuid == Uuid);

			return DBAllowUser != null;
		}

		internal static bool AllowUserFind(ulong GuildID, string Name, [MaybeNullWhen(true)] out AllowUsers DBAllowUser) {
			DBAllowUser = AllowUsersCollection.FindOne(AllowUser => AllowUser.GuildID == GuildID & AllowUser.Name == Name);

			return DBAllowUser != null;
		}

		internal static bool AllowUsersListFind(ulong GuildID, [MaybeNullWhen(true)] out List<AllowUsers> DBAllowUsers) {
			DBAllowUsers = AllowUsersCollection.Find(AllowUser => AllowUser.GuildID == GuildID).ToList();

			return DBAllowUsers != null;
		}

		internal static bool AllowUserUpdate(ulong GuildID, ulong Uuid, string Name = null, uint? RoleNum = null) {
			if (AllowUserFind(GuildID, Uuid, out AllowUsers DBAllowUser)) {
				Name ??= DBAllowUser.Name;
				RoleNum ??= DBAllowUser.RoleNum;

				DBAllowUser.Name = Name;
				DBAllowUser.RoleNum = (uint) RoleNum;

				AllowUsersCollection.Update(DBAllowUser);

				return true;
			}
			else return false;
		}

		internal static bool AllowUserUpdate(AllowUsers DBAllowUser) => AllowUsersCollection.Update(DBAllowUser);

		internal static AllowUsers AllowUserInsert(ulong GuildID, ulong Uuid, string Name, uint RoleNum) {
			AllowUsers InsertAllowUser = new AllowUsers {
				GuildID = GuildID,
				Uuid = Uuid,
				Name = Name,
				RoleNum = RoleNum
			};

			AllowUsersCollection.Insert(InsertAllowUser);

			return InsertAllowUser;
		}

		internal static bool AllowUserDelete(AllowUsers AllowUser) => AllowUsersCollection.Delete(AllowUser.Id);
	}
}
