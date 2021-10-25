using Avespoir.Core.Database.Schemas;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class AllowUsersMethods {

		internal static bool AllowUserExist(ulong GuildID, ulong Uuid) => AllowUserFind(GuildID, Uuid, out _);

		internal static bool AllowUserExist(ulong GuildID, string Name) => AllowUserFind(GuildID, Name, out _);

		internal static bool AllowUserFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(true)] out AllowUsers DBAllowUser) {
			DBAllowUser = (
				from AllowUser in MongoDBClient.AllowUsersCollection.AsQueryable()
				where AllowUser.GuildID == GuildID
				where AllowUser.Uuid == Uuid
				select AllowUser
			).FirstOrDefault();

			return DBAllowUser != null;
		}

		internal static bool AllowUserFind(ulong GuildID, string Name, [MaybeNullWhen(true)] out AllowUsers DBAllowUser) {
			DBAllowUser = (
				from AllowUser in MongoDBClient.AllowUsersCollection.AsQueryable()
				where AllowUser.GuildID == GuildID
				where AllowUser.Name == Name
				select AllowUser
			).FirstOrDefault();

			return DBAllowUser != null;
		}

		internal static bool AllowUsersListFind(ulong GuildID, [MaybeNullWhen(true)] out List<AllowUsers> DBAllowUsers) {
			DBAllowUsers = (
				from AllowUser in MongoDBClient.AllowUsersCollection.AsQueryable()
				where AllowUser.GuildID == GuildID
				select AllowUser
			).ToList();

			return DBAllowUsers != null;
		}

		internal static bool AllowUserUpdate(ulong GuildID, ulong Uuid, string Name = null, uint? RoleNum = null) {
			if (AllowUserFind(GuildID, Uuid, out AllowUsers DBAllowUser)) {
				Name ??= DBAllowUser.Name;
				RoleNum ??= DBAllowUser.RoleNum;

				UpdateDefinition<AllowUsers> UpdateDef = Builders<AllowUsers>.Update
					.Set(AllowUser => AllowUser.Name, Name)
					.Set(AllowUser => AllowUser.RoleNum, (uint) RoleNum);
				MongoDBClient.AllowUsersCollection.UpdateOne(AllowUser => AllowUser.Id == DBAllowUser.Id, UpdateDef);

				return true;
			}
			else return false;
		}

		internal static bool AllowUserUpdate(AllowUsers DBAllowUser) =>
			AllowUserUpdate(DBAllowUser.GuildID, DBAllowUser.Uuid, DBAllowUser.Name, DBAllowUser.RoleNum);

		internal static AllowUsers AllowUserInsert(ulong GuildID, ulong Uuid, string Name, uint RoleNum) {
			AllowUsers InsertAllowUser = new AllowUsers {
				GuildID = GuildID,
				Uuid = Uuid,
				Name = Name,
				RoleNum = RoleNum
			};

			MongoDBClient.AllowUsersCollection.InsertOne(InsertAllowUser);

			return InsertAllowUser;
		}

		internal static bool AllowUserDelete(AllowUsers AllowUser) {
			MongoDBClient.AllowUsersCollection.DeleteOne(Allow_User => Allow_User.Id == AllowUser.Id);
			return true;
		}
	}
}
