using Avespoir.Core.Database.Schemas;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class RolesMethods {

		private static Roles RoleFind(ulong GuildID, ulong Uuid) => (
			from Role in MongoDBClient.RolesCollection.AsQueryable()
			where Role.GuildID == GuildID
			where Role.Uuid == Uuid
			select Role
		).FirstOrDefault();

		private static Roles RoleFind(ulong GuildID, uint RoleNum) => (
			from Role in MongoDBClient.RolesCollection.AsQueryable()
			where Role.GuildID == GuildID
			where Role.RoleNum == RoleNum
			select Role
		).FirstOrDefault();

		private static Roles RoleFind(ulong GuildID, Enums.RoleLevel RoleLevel) => (
			from Role in MongoDBClient.RolesCollection.AsQueryable()
			where Role.GuildID == GuildID
			where Role.RoleLevel == RoleLevel
			select Role
		).FirstOrDefault();

		internal static bool RoleFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = RoleFind(GuildID, Uuid);

			return DBRole != null;
		}

		internal static bool RoleFind(ulong GuildID, uint RoleNum, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = RoleFind(GuildID, RoleNum);

			return DBRole != null;
		}

		internal static bool RoleFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out Roles DBRole) {
			DBRole = RoleFind(GuildID, RoleLevel);

			return DBRole != null;
		}

		internal static bool RoleExist(ulong GuildID, ulong Uuid) => RoleFind(GuildID, Uuid, out _);

		internal static bool RoleExist(ulong GuildID, uint RoleNum) => RoleFind(GuildID, RoleNum, out _);

		internal static bool RoleExist(ulong GuildID, Enums.RoleLevel RoleLevel) => RoleFind(GuildID, RoleLevel, out _);

		internal static bool RolesListFind(ulong GuildID, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			DBRoles = (
				from Role in MongoDBClient.RolesCollection.AsQueryable()
				where Role.GuildID == GuildID
				select Role
			).ToList();

			return DBRoles != null;
		}

		internal static bool RolesListFind(ulong GuildID, Enums.RoleLevel RoleLevel, [MaybeNullWhen(true)] out List<Roles> DBRoles) {
			DBRoles = (
					from Role in MongoDBClient.RolesCollection.AsQueryable()
					where Role.GuildID == GuildID
					where Role.RoleLevel == RoleLevel
					select Role
				).ToList();

			return DBRoles != null;
		}

		internal static bool RoleUpdate(ulong GuildID, ulong Uuid, uint RoleNum, Enums.RoleLevel RoleLevel) {
			if (RoleFind(GuildID, Uuid, out Roles DBRole)) {
				UpdateDefinition<Roles> UpdateDef = Builders<Roles>.Update
					.Set(Role => Role.RoleNum, RoleNum)
					.Set(Role => Role.RoleLevel, RoleLevel);
				MongoDBClient.RolesCollection.UpdateOne(Role => Role.Id == DBRole.Id, UpdateDef);

				return true;
			}
			else return false;
		}

		internal static Roles RoleInsert(ulong GuildID, ulong Uuid, uint RoleNum, Enums.RoleLevel RoleLevel) {
			Roles InsertRole = new Roles {
				GuildID = GuildID,
				Uuid = Uuid,
				RoleNum = RoleNum,
				RoleLevel = RoleLevel
			};

			MongoDBClient.RolesCollection.InsertOne(InsertRole);

			return InsertRole;
		}

		internal static bool RoleDelete(Roles Role) {
			MongoDBClient.RolesCollection.DeleteOne(Role_ => Role_.Id == Role.Id);

			return true;
		}
	}
}
