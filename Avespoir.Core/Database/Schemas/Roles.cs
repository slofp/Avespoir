using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	class Roles {

		[BsonId]
		public ObjectId id { get; set; }

		[BsonElement("Guildid")]
		public ulong GuildID { get; set; }

		[BsonElement("uuid")]
		public ulong uuid { get; set; }

		[BsonElement("RoleNum")]
		public uint RoleNum { get; set; }

		[BsonElement("RoleLevel")]
		public string RoleLevel { get; set; }
	}
}
