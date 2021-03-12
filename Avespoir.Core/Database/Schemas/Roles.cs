using LiteDB;

namespace Avespoir.Core.Database.Schemas {

	class Roles {

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonField("Guildid")]
		public ulong GuildID { get; set; }

		[BsonField("uuid")]
		public ulong Uuid { get; set; }

		[BsonField("RoleNum")]
		public uint RoleNum { get; set; }

		[BsonField("RoleLevel")]
		public string RoleLevel { get; set; }
	}
}
