using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avespoir.Core.Database.Schemas {

	class UserData {

		[BsonId]
		public ObjectId id { get; set; }

		[BsonElement("User_id")]
		public ulong UserID { get; set; }

		[BsonElement("Level")]
		public uint Level { get; set; }

		[BsonElement("Experience-point")]
		public double ExperiencePoint { get; set; }

		
	}
}
