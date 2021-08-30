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

		internal static Roles FindOne(Func<Roles, bool> WhereFunc) {
			try {
				MySqlClient.Database.BeginTransaction();
				Roles Result = RolesTable.Where(WhereFunc).FirstOrDefault();
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
			try {
				MySqlClient.Database.BeginTransaction();
				DBRoles = (
					from Role in RolesTable
					where Role.GuildID == GuildID
					select Role
				).ToList();

				MySqlClient.Database.CommitTransaction();

				return DBRoles != null;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return RolesListFind(GuildID, out DBRoles);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				throw;
			}
		}

		internal static bool RolesListFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			try {
				MySqlClient.Database.BeginTransaction();
				DBRoles = (
					from Role in RolesTable
					where Role.GuildID == GuildID
					where Role.RoleLevel == Enum.GetName(typeof(Enums.RoleLevel), RoleLevel)
					select Role
				).ToList();
				MySqlClient.Database.CommitTransaction();

				return DBRoles != null;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return RolesListFind(GuildID, RoleLevel, out DBRoles);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				throw;
			}
		}

		internal static bool RoleUpdate(ulong GuildID, ulong Uuid, uint RoleNum, string RoleLevel) {
			try {
				if (RoleFind(GuildID, Uuid, out Roles DBRole)) {
					MySqlClient.Database.BeginTransaction();
					RolesTable
					.Where(Role => Role.Id == DBRole.Id)
					.Set(Role => Role.RoleNum, RoleNum)
					.Set(Role => Role.RoleLevel, RoleLevel)
					.Update();

					MySqlClient.Database.CommitTransaction();

					return true;
				}
				else return false;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return RoleUpdate(GuildID, Uuid, RoleNum, RoleLevel);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				return false;
			}
		}

		internal static Roles RoleInsert(ulong GuildID, ulong Uuid, uint RoleNum, string RoleLevel) {
			try {
				Roles InsertRole = new Roles {
					GuildID = GuildID,
					Uuid = Uuid,
					RoleNum = RoleNum,
					RoleLevel = RoleLevel
				};

				MySqlClient.Database.BeginTransaction();
				RolesTable
				.Value(x => x.GuildID, InsertRole.GuildID)
				.Value(x => x.Uuid, InsertRole.Uuid)
				.Value(x => x.RoleNum, InsertRole.RoleNum)
				.Value(x => x.RoleLevel, InsertRole.RoleLevel)
				.Insert();

				MySqlClient.Database.CommitTransaction();

				return InsertRole;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return RoleInsert(GuildID, Uuid, RoleNum, RoleLevel);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();

				throw;
			}
		}

		internal static bool RoleDelete(Roles Role) {
			try {
				MySqlClient.Database.BeginTransaction();
				RolesTable.Where(Role_ => Role_.Id == Role.Id).Delete();
				MySqlClient.Database.CommitTransaction();
				return true;
			}
			catch (MySql.Data.MySqlClient.MySqlException) {
				MySqlClient.DBUpdate();

				return RoleDelete(Role);
			}
			catch (Exception) {
				MySqlClient.Database.RollbackTransaction();
				return false;
			}
		}
	}
}
