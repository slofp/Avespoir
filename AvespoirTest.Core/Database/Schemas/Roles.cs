using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AvespoirTest.Core.Database.Schemas {

	class Roles {

		[BsonId]
		public ObjectId id { get; set; }

		[BsonElement("Name")]
		public string Name { get; set; }

		[BsonElement("uuid")]
		public ulong uuid { get; set; }

		[BsonElement("RoleDef")]
		public uint RoleDef { get; set; }
	}
}
