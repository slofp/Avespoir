using LiteDB;

namespace Avespoir.Core.Database.Schemas {

	class AllowUsers {
		
		[BsonId]
		public ObjectId Id { get; set; }

		[BsonField("Guildid")]
		public ulong GuildID { get; set; }

		[BsonField("Name")]
		public string Name { get; set; }

		[BsonField("uuid")]
		public ulong Uuid { get; set; }

		[BsonField("RoleNum")]
		public uint RoleNum { get; set; }
	}
}
