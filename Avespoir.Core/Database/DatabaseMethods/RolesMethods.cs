using Avespoir.Core.Database.Schemas;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class RolesMethods {

		internal static ITable<Roles> RolesTable =>
			MySqlClient.Database.GetTable<Roles>();

		internal static Roles FindOne(Func<Roles, bool> WhereFunc) => (
			 from Role in RolesTable
			 where WhereFunc(Role)
			 select Role
			).FirstOrDefault();

		internal static bool RoleExist(ulong GuildID, ulong Uuid) => RoleFind(GuildID, Uuid, out Roles _);

		internal static bool RoleExist(ulong GuildID, uint RoleNum) => RoleFind(GuildID, RoleNum, out Roles _);

		internal static bool RoleExist(ulong GuildID, Enums.RoleLevel RoleLevel) => RoleFind(GuildID, RoleLevel, out Roles _);

		internal static bool RoleFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = FindOne(Role => Role.GuildID == GuildID & Role.Uuid == Uuid);

			return DBRole != null;
		}

		internal static bool RoleFind(ulong GuildID, uint RoleNum, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = FindOne(Role => Role.GuildID == GuildID & Role.RoleNum == RoleNum);

			return DBRole != null;
		}

		internal static bool RoleFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = FindOne(Role => Role.GuildID == GuildID & Role.RoleLevel == Enum.GetName(typeof(Enums.RoleLevel), RoleLevel));

			return DBRole != null;
		}

		internal static bool RolesListFind(ulong GuildID, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			DBRoles = (
				from Role in RolesTable
				where Role.GuildID == GuildID
				select Role
			).ToList();

			return DBRoles != null;
		}

		internal static bool RolesListFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			DBRoles = (
				from Role in RolesTable
				where Role.GuildID == GuildID
				where Role.RoleLevel == Enum.GetName(typeof(Enums.RoleLevel), RoleLevel)
				select Role
			).ToList();

			return DBRoles != null;
		}

		internal static bool RoleUpdate(ulong GuildID, ulong Uuid, uint RoleNum, string RoleLevel) {
			if (RoleFind(GuildID, Uuid, out Roles DBRole)) {

				RolesTable
				.Where(Role => Role.Id == DBRole.Id)
				.Set(Role => Role.RoleNum, RoleNum)
				.Set(Role => Role.RoleLevel, RoleLevel)
				.Update();

				return true;
			}
			else return false;
		}

		internal static Roles RoleInsert(ulong GuildID, ulong Uuid, uint RoleNum, string RoleLevel) {
			Roles InsertRole = new Roles {
				GuildID = GuildID,
				Uuid = Uuid,
				RoleNum = RoleNum,
				RoleLevel = RoleLevel
			};

			RolesTable
			.Value(x => x.GuildID, InsertRole.GuildID)
			.Value(x => x.Uuid, InsertRole.Uuid)
			.Value(x => x.RoleNum, InsertRole.RoleNum)
			.Value(x => x.RoleLevel, InsertRole.RoleLevel)
			.Insert();

			return InsertRole;
		}

		internal static bool RoleDelete(Roles Role) {
			try {
				RolesTable.Where(Role_ => Role_.Id == Role.Id).Delete();
				return true;
			}
			catch (Exception) {
				return false;
			}
		}
	}
}
