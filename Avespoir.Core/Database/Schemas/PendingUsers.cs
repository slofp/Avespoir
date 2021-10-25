using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Avespoir.Core.Database.Schemas {

	public class PendingUsers {

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

		[BsonElement("Messageid")]
		public ulong MessageID { get; set; }

		[BsonDateTimeOptions(DateOnly = false, Kind = DateTimeKind.Local)]
		public DateTime PendingStart { get; set; }

	}
}
