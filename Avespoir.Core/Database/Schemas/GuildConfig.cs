using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	class GuildConfig {

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("Guildid")]
		public ulong GuildID { get; set; }

		[BsonElement("WhiteList")]
		public bool WhiteList { get; set; }

		[BsonElement("LeaveBan")]
		public bool LeaveBan { get; set; }

		[BsonElement("PublicPrefix")]
		public string PublicPrefix { get; set; }

		[BsonElement("ModeratorPrefix")]
		public string ModeratorPrefix { get; set; }

		[BsonElement("LogChannelId")]
		public ulong LogChannelId { get; set; }

		[BsonElement("Language")]
		public string Language { get; set; }
	}
}
