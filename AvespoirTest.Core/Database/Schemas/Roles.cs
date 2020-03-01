using MongoDB.Bson;

namespace AvespoirTest.Core.Database.Schemas {

	class Roles {

		public ObjectId id { get; set; }

		public string Name { get; set; }

		public ulong uuid { get; set; }

		public uint RoleDef { get; set; }
	}
}
