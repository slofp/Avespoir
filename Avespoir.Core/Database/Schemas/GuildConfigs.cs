using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	class GuildConfigs {

		[BsonId]
		public ObjectId id { get; set; }

		[BsonElement("Guild_id")]
		public ulong GuildID { get; set; }

		[BsonElement("Public_prefix")]
		public string PublicPrefix { get; set; }

		[BsonElement("Moderator_prefix")]
		public string ModeratorPrefix { get; set; }

		[BsonElement("Whitelist")]
		public bool Whitelist { get; set; }

		[BsonElement("LeftBAN")]
		public bool LeftBAN { get; set; }
	}
}
