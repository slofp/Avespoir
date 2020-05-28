using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	class AllowUsers {
		
		[BsonId]
		public ObjectId id { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("uuid")]
		public ulong uuid { get; set; }

		[BsonElement("RoleNum")]
		public uint RoleNum { get; set; }
	}
}
