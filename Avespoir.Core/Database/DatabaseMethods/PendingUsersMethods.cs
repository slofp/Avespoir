using Avespoir.Core.Database.Schemas;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Avespoir.Core.Database.DatabaseMethods {

	class PendingUsersMethods {

		internal static bool PendingUserExist(ulong GuildID, ulong Uuid) => PendingUserFind(GuildID, Uuid, out _);

		internal static bool PendingUserExist(ulong GuildID, string Name) => PendingUserFind(GuildID, Name, out _);

		private static bool PendingUserFind(ulong GuildID, ulong Uuid, [MaybeNullWhen(false)] out PendingUsers DBPendingUser) {
			DBPendingUser = (
				from PendingUser in MongoDBClient.PendingUsersCollection.AsQueryable()
				where PendingUser.GuildID == GuildID
				where PendingUser.Uuid == Uuid
				select PendingUser
			).FirstOrDefault();

			return DBPendingUser != null;
		}

		private static bool PendingUserFind(ulong GuildID, string Name, [MaybeNullWhen(false)] out PendingUsers DBPendingUser) {
			DBPendingUser = (
				from PendingUser in MongoDBClient.PendingUsersCollection.AsQueryable()
				where PendingUser.GuildID == GuildID
				where PendingUser.Name == Name
				select PendingUser
			).FirstOrDefault();

			return DBPendingUser != null;
		}

		internal static bool PendingUserFind(ulong MessageID, [MaybeNullWhen(false)] out PendingUsers DBPendingUser) {
			DBPendingUser = (
				from PendingUser in MongoDBClient.PendingUsersCollection.AsQueryable()
				where PendingUser.MessageID == MessageID
				select PendingUser
			).FirstOrDefault();

			return DBPendingUser != null;
		}

		internal static List<PendingUsers> PendingUserList() => (
			from PendingUser in MongoDBClient.PendingUsersCollection.AsQueryable()
			select PendingUser
		).ToList();

		internal static PendingUsers PendingUserInsert(ulong GuildID, ulong Uuid, string Name, uint RoleNum, ulong MessageID) {
			PendingUsers InsertPendingUser = new PendingUsers {
				GuildID = GuildID,
				Uuid = Uuid,
				Name = Name,
				RoleNum = RoleNum,
				MessageID = MessageID,
				PendingStart = DateTime.Now
			};

			MongoDBClient.PendingUsersCollection.InsertOne(InsertPendingUser);

			return InsertPendingUser;
		}

		internal static bool PendingUserDelete(PendingUsers PendingUser) {
			MongoDBClient.PendingUsersCollection.DeleteOne(Allow_User => Allow_User.Id == PendingUser.Id);
			return true;
		}
	}
}
