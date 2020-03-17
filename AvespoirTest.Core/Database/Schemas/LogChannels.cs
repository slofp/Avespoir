using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AvespoirTest.Core.Database.Schemas {

	class LogChannels {

		[BsonId]
		public ObjectId id { get; set; }

		[BsonElement("Guildid")]
		public ulong GuildID { get; set; }

		[BsonElement("Channnelid")]
		public ulong ChannelID { get; set; }
	}
}
