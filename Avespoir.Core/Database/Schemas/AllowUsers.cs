using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	public class AllowUsers {

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("Guildid")]
		public ulong GuildID { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("uuid")]
		public ulong Uuid { get; set; }

		[BsonElement("RoleNum")]
		public uint RoleNum { get; set; }
	}
}
