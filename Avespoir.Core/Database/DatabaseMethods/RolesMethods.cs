using Avespoir.Core.Database.Schemas;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class RolesMethods {

		internal static ILiteCollection<Roles> RolesCollection =>
			LiteDBClient.Database.GetCollection<Roles>(typeof(Roles).Name);

		internal static bool RoleExist(ulong GuildID, ulong Uuid) => RoleFind(GuildID, Uuid, out Roles _);

		internal static bool RoleExist(ulong GuildID, uint RoleNum) => RoleFind(GuildID, RoleNum, out Roles _);

		internal static bool RoleExist(ulong GuildID, Enums.RoleLevel RoleLevel) => RoleFind(GuildID, RoleLevel, out Roles _);

		internal static bool RoleFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = RolesCollection.FindOne(Role => Role.GuildID == GuildID & Role.Uuid == Uuid);

			return DBRole != null;
		}

		internal static bool RoleFind(ulong GuildID, uint RoleNum, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = RolesCollection.FindOne(Role => Role.GuildID == GuildID & Role.RoleNum == RoleNum);

			return DBRole != null;
		}

		internal static bool RoleFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = RolesCollection.FindOne(Role => Role.GuildID == GuildID & Role.RoleLevel == Enum.GetName(typeof(Enums.RoleLevel), RoleLevel));

			return DBRole != null;
		}

		internal static bool RolesListFind(ulong GuildID, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			DBRoles = RolesCollection.Find(Role => Role.GuildID == GuildID).ToList();

			return DBRoles != null;
		}

		internal static bool RolesListFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			DBRoles = RolesCollection.Find(Role => Role.GuildID == GuildID & Role.RoleLevel == Enum.GetName(typeof(Enums.RoleLevel), RoleLevel)).ToList();

			return DBRoles != null;
		}

		internal static bool RoleUpdate(ulong GuildID, ulong Uuid, uint RoleNum, string RoleLevel) {
			if (RoleFind(GuildID, Uuid, out Roles DBRole)) {

				DBRole.RoleNum = RoleNum;
				DBRole.RoleLevel = RoleLevel;

				RolesCollection.Update(DBRole);

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

			RolesCollection.Insert(InsertRole);

			return InsertRole;
		}

		internal static bool RoleDelete(Roles Role) => RolesCollection.Delete(Role.Id);
	}
}
