using LiteDB;

namespace Avespoir.Core.Database.Schemas {

	class UserData {

		[BsonId]
		public ObjectId Id { get; set; }

		[BsonField("User_id")]
		public ulong UserID { get; set; }

		[BsonField("Level")]
		public uint Level { get; set; }

		[BsonField("Experience-point")]
		public double ExperiencePoint { get; set; }

		
	}
}
