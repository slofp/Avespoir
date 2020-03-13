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

		[BsonElement("RoleNum")]
		public uint RoleNum { get; set; }

		[BsonElement("RoleLevel")]
		public string RoleLevel { get; set; }
	}
}
