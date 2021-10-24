using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	public class GuildConfig {

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonElement("Guildid")]
		public ulong GuildID { get; set; }

		[BsonElement("WhiteList")]
		public bool WhiteList { get; set; }

		[BsonElement("LeaveBan")]
		public bool LeaveBan { get; set; }

		[BsonElement("Prefix")]
		public string Prefix { get; set; }

		[BsonElement("LogChannelId")]
		public ulong LogChannelId { get; set; }

		[BsonElement("Language"), BsonRepresentation(BsonType.String)]
		public Enums.Language Language { get; set; }

		[BsonElement("LevelSwitch")]
		public bool LevelSwitch { get; set; }
	}
}
